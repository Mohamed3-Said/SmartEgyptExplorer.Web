using AutoMapper;
using DomainLayer.Contracts.Repo;
using DomainLayer.Engines;
using DomainLayer.Models.Models.PlanModule;
using DomainLayer.Models.PlaceModule;
using DomainLayer.Models.PlanModule;
using DomainLayer.Models.Remaining_Modules;
using ServiceAbstraction.Services;
using Shared.DTOS.APIFormsDTOs;
using Shared.DTOS.APIFormsDTOs.AIDTOs;
using System.Text;

namespace Service.ServiceImplemmentation
{
    public class FormService : IFormService
    {
        private readonly IUserFormRepository _formRepository;
        private readonly IPlanRepository _planRepository;
        private readonly IAIService _aiService;
        private readonly IMapper _mapper;

        public FormService(
            IUserFormRepository formRepository,
            IPlanRepository planRepository,
            IAIService aiService,
            IMapper mapper)
        {
            _formRepository = formRepository;
            _planRepository = planRepository;
            _aiService = aiService;
            _mapper = mapper;
        }

        // 1️⃣ استلام الفورم وحفظها
        public async Task<int> SubmitFormAsync(string userId, IEnumerable<FormAnswerDto> answers)
        {
            var submission = new UserFormSubmission
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UserAnswers = answers.Select(a => new UserAnswer
                {
                    QuestionKey = a.Key,
                    AnswerValue = a.Value
                }).ToList()
            };

            await _formRepository.AddSubmissionAsync(submission);
            return submission.UserFormSubmissionId;
        }

        // 2️⃣ توليد الخطة (الاعتماد الكلي على بايثون)
        public async Task<PlanDto> GeneratePlanAsync(int submissionId)
        {
            var submission = await _formRepository.GetSubmissionWithAnswersAsync(submissionId);
            if (submission == null) throw new Exception("Submission not found");

            // سحب القيم من الإجابات بطريقة مرنة
            var city = submission.UserAnswers.FirstOrDefault(a => a.QuestionKey == "City")?.AnswerValue ?? "Cairo";
            var budgetStr = submission.UserAnswers.FirstOrDefault(a => a.QuestionKey == "BudgetAmount")?.AnswerValue ?? "500";
            var nationality = submission.UserAnswers.FirstOrDefault(a => a.QuestionKey == "Nationality")?.AnswerValue ?? "Foreigner";
            var tier = submission.UserAnswers.FirstOrDefault(a => a.QuestionKey == "Tier")?.AnswerValue ?? "Standard";
            var adultsStr = submission.UserAnswers.FirstOrDefault(a => a.QuestionKey == "Adults")?.AnswerValue ?? "1";

            var interests = submission.UserAnswers
               .Where(a => a.QuestionKey == "Interests")
               .Select(a => a.AnswerValue)
               .ToList();

            // بناء الـ Payload المطابق لطلبات محمود (بايثون)
            var travelData = new
            {
                // الحقول اللي محمود بيعمل لها extract في الدالة بتاعته
                arrivalDateTime = DateTime.Now.AddDays(7).ToString("yyyy-MM-ddTHH:mm:ss"),
                departureDateTime = DateTime.Now.AddDays(12).ToString("yyyy-MM-ddTHH:mm:ss"),
                cityArrival = city.Trim(),
                cityDeparture = city.Trim(),
                nationality = nationality,
                adults = int.TryParse(adultsStr, out var acc) ? acc : 1,
                students = 0,
                children_6_12 = 0,
                children_under_4 = 0,
                special_needs_count = 0,
                budgetAmount = double.TryParse(budgetStr, out var b) ? b : 500.0,
                budgetType = "Total Trip",
                tier = tier,
                pacing = "Balanced",
                interests = interests.Any() ? interests : new List<string> { "History" },
                inter_city_transport = "Uber",
                food_type = "Local",
                include_food = true,
                show_recipe_details = true,

                // الحقول اللي كانت ناقصة ومسببة KeyError في كود محمود
                visited_before = false, // تأكد إنها lowercase وموجودة
                guide_needed = false,
                accessibility_needs = false,
                restrictions = new List<string>(),
                cities_to_visit = new List<string> { city.Trim() }
            };

            // ✅ حل الخطأ CS1503: التأكد أن الخدمة ترجع Object DTO
            AIPlanResponseDto aiResponse = await _aiService.GeneratePlanAsync(travelData);
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(aiResponse));
            Console.WriteLine($"AI Response Days Count: {aiResponse.Days?.Count}");
            if (aiResponse == null || aiResponse.Status != "success")
                throw new Exception("AI Engine failed to generate a valid plan.");

            var planEntity = MapAiResponseToPlanEntity(aiResponse, submission.UserId, city);
            var savedPlan = await _planRepository.CreatePlanAsync(planEntity);

            return _mapper.Map<PlanDto>(savedPlan);
        }

        // 3️⃣ الـ Mapping الاحترافي للـ Entity
        private Plan MapAiResponseToPlanEntity(AIPlanResponseDto aiResponse, string userId, string city)
        {
            decimal calculatedActivitiesCost = aiResponse.Days
                 .SelectMany(d => d.Activities)
                 .Sum(a => (decimal)a.Cost);

            decimal calculatedTransportCost = aiResponse.Days
                .SelectMany(d => d.Activities)
                .Sum(a => (decimal)a.TransportCost);

            var totalTripCost = calculatedActivitiesCost + calculatedTransportCost;
            var plan = new Plan
            {
                UserId = userId,
                City = city,
                TotalPriceEGP = aiResponse.TotalEstimatedCost,
                TotalEstimatedCost = aiResponse.TotalEstimatedCost,
                CreatedAt = DateTime.UtcNow,
                StartDate = DateTime.Now.AddDays(7),
                EndDate = DateTime.Now.AddDays(12),
                BudgetLevel = "Medium",
                PreferredTransport = "Uber",

                PlanDays = aiResponse.Days.Select(d => new PlanDay
                {
                    DayNumber = d.DayNumber,
                    Date = DateTime.TryParse(d.Date, out var dt) ? dt : DateTime.Now,
                    MustTryDishTitle = d.MustTryFood?.Title,
                    MustTryDishDesc = d.MustTryFood?.Description,
                    MustTryFood = d.MustTryFood?.Title, // للحفاظ على الحقل القديم لو محتاجه

                    PlanActivities = d.Activities.Select(a => new PlanActivity
                    {
                        Title = a.Title,
                        Description = a.Description,
                        Category = a.Description,
                        Cost = a.Cost,
                        Latitude = a.Lat,
                        Longitude = a.Lng,
                        ImageURL = a.ImageURL,
                        TransportCost = a.TransportCost,
                        // ✅ حل مشكلة الـ TimeSpan (الخطأ CS0029)
                        StartTime = DateTime.TryParse(a.StartTime, out var st) ? st.TimeOfDay : TimeSpan.FromHours(10),
                        EndTime = DateTime.TryParse(a.StartTime, out var et) ? et.AddHours(2).TimeOfDay : TimeSpan.FromHours(12),
                        RecommendedTransport = "Uber",
                        RecommendedFood = "Local"
                    }).ToList()
                }).ToList()
            };

            return plan;
        }

        public async Task<bool> DeletePlanAsync(int planId, string userId)
        {

            return await _planRepository.DeletePlanAsync(planId, userId);
        }

        public async Task<PlanDto?> GetUserCurrentPlanAsync(string userId)
        {
            var plan = await _planRepository.GetCurrentPlanAsync(userId);
            if (plan == null) return null;

            // تحويل الـ Plan لـ PlanDto (أنا مفترض إن عندك AutoMapper أو تحويل يدوي)
            return _mapper.Map<PlanDto>(plan);
        }

        public async Task<IEnumerable<PlanDto>> GetUserHistoryAsync(string userId)
        {
            var plans = await _planRepository.GetUserPlansHistoryAsync(userId);
            return _mapper.Map<IEnumerable<PlanDto>>(plans);
        }

        public async Task<PlanDto?> GetPlanDetailsAsync(int planId, string userId)
        {
            var plan = await _planRepository.GetPlanByIdAsync(planId, userId);
            return _mapper.Map<PlanDto>(plan);
        }
    }
}
