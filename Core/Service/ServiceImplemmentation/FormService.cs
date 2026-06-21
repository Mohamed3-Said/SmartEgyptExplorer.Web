using AutoMapper;
using DomainLayer.Contracts.Repo;
using DomainLayer.Engines;
using DomainLayer.Models.Models.PlanModule;
using DomainLayer.Models.PlaceModule;
using DomainLayer.Models.PlanModule;
using DomainLayer.Models.Remaining_Modules;
using Microsoft.EntityFrameworkCore;
using ServiceAbstraction.Services;
using Shared.DTOS.APIFormsDTOs;
using Shared.DTOS.APIFormsDTOs.AIDTOs;
using Shared.DTOS.APIFormsDTOs.AIDTOs.DetailsDTOS;
using System.Text;
using System.Text.Json;

namespace Service.ServiceImplemmentation
{
    public class FormService : IFormService
    {
        private readonly IUserFormRepository _formRepository;
        private readonly IPlanRepository _planRepository;
        private readonly IAIService _aiService;
        private readonly IMapper _mapper;
        private readonly IHotelRepository _hotelRepository;

        public FormService(
            IUserFormRepository formRepository,
            IPlanRepository planRepository,
            IAIService aiService,
            IMapper mapper,
            IHotelRepository hotelRepository
            )
        {
            _formRepository = formRepository;
            _planRepository = planRepository;
            _aiService = aiService;
            _mapper = mapper;
            _hotelRepository = hotelRepository;
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

        // ==========================================
        // استبدل الـ GeneratePlanAsync method بالكامل
        // ==========================================
        public async Task<PlanDto> GeneratePlanAsync(int submissionId)
        {
            var submission = await _formRepository.GetSubmissionWithAnswersAsync(submissionId);
            if (submission == null) throw new Exception("Submission not found");

            var userAnswers = submission.UserAnswers;

            // ✅ Keys مطابقة بالظبط للـ FormAnswerDto اللي بيبعته الـ Frontend
            var city = userAnswers.FirstOrDefault(a => a.QuestionKey == "cityArrival")?.AnswerValue ?? "Cairo";
            var budgetStr = userAnswers.FirstOrDefault(a => a.QuestionKey == "budgetAmount")?.AnswerValue ?? "5000";
            var nationality = userAnswers.FirstOrDefault(a => a.QuestionKey == "nationality")?.AnswerValue ?? "Egyptian";
            var tier = userAnswers.FirstOrDefault(a => a.QuestionKey == "tier")?.AnswerValue ?? "Standard";
            var adultsStr = userAnswers.FirstOrDefault(a => a.QuestionKey == "adults")?.AnswerValue ?? "1";
            var studentsStr = userAnswers.FirstOrDefault(a => a.QuestionKey == "students")?.AnswerValue ?? "0";
            var children612Str = userAnswers.FirstOrDefault(a => a.QuestionKey == "children_6_12")?.AnswerValue ?? "0";
            var childrenUnder4Str = userAnswers.FirstOrDefault(a => a.QuestionKey == "children_under_4")?.AnswerValue ?? "0";
            var specialNeedsStr = userAnswers.FirstOrDefault(a => a.QuestionKey == "special_needs_count")?.AnswerValue ?? "0";
            var pacing = userAnswers.FirstOrDefault(a => a.QuestionKey == "pacing")?.AnswerValue ?? "Medium";
            // var foodType = userAnswers.FirstOrDefault(a => a.QuestionKey == "food_type")?.AnswerValue ?? "Local";
            var foodTypeRaw = userAnswers.FirstOrDefault(a => a.QuestionKey == "food_type")?.AnswerValue ?? "Local";

            var foodType = foodTypeRaw
                .Split(',')
                .Select(f => f.Trim())
                .Where(f => !string.IsNullOrEmpty(f))
                .ToList();
            var budgetType = userAnswers.FirstOrDefault(a => a.QuestionKey == "budgetType")?.AnswerValue ?? "Total Trip";
            var interTransport = userAnswers.FirstOrDefault(a => a.QuestionKey == "inter_city_transport")?.AnswerValue ?? "Uber";
            var includeFood = userAnswers.FirstOrDefault(a => a.QuestionKey == "include_food")?.AnswerValue == "true";
            var guideNeeded = userAnswers.FirstOrDefault(a => a.QuestionKey == "guide_needed")?.AnswerValue == "true";
            var visitedBefore = userAnswers.FirstOrDefault(a => a.QuestionKey == "visited_before")?.AnswerValue == "true";
            var accessibilityNeeds = userAnswers.FirstOrDefault(a => a.QuestionKey == "accessibility_needs")?.AnswerValue == "true";
            var showRecipe = userAnswers.FirstOrDefault(a => a.QuestionKey == "show_recipe_details")?.AnswerValue == "true";

            var arrivalStr = userAnswers.FirstOrDefault(a => a.QuestionKey == "arrivalDateTime")?.AnswerValue;
            var departureStr = userAnswers.FirstOrDefault(a => a.QuestionKey == "departureDateTime")?.AnswerValue;

            DateTime arrivalDate = DateTime.TryParse(arrivalStr, out var start) ? start : DateTime.Now.AddDays(1);
            DateTime departureDate = DateTime.TryParse(departureStr, out var end) ? end : arrivalDate.AddDays(3);

            // cities_to_visit و interests جايين كـ string مفصول بـ comma من الـ Frontend
            var citiesRaw = userAnswers.FirstOrDefault(a => a.QuestionKey == "cities_to_visit")?.AnswerValue ?? city;
            var interestsRaw = userAnswers.FirstOrDefault(a => a.QuestionKey == "interests")?.AnswerValue ?? "History";
            var restrictionsRaw = userAnswers.FirstOrDefault(a => a.QuestionKey == "restrictions")?.AnswerValue ?? "";

            var citiesList = citiesRaw.Split(',').Select(c => c.Trim()).Where(c => !string.IsNullOrEmpty(c)).ToList();
            var interestsList = interestsRaw.Split(',').Select(i => i.Trim()).Where(i => !string.IsNullOrEmpty(i)).ToList();
            var restrictionsList = string.IsNullOrEmpty(restrictionsRaw)
                ? new List<string>()
                : restrictionsRaw.Split(',').Select(r => r.Trim()).Where(r => !string.IsNullOrEmpty(r)).ToList();

            // ✅ TripRequest مطابق بالظبط لـ FastAPI
            var travelData = new TripRequest
            {
                arrivalDateTime = arrivalDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                departureDateTime = departureDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                cityArrival = city.Trim(),
                cityDeparture = userAnswers.FirstOrDefault(a => a.QuestionKey == "cityDeparture")?.AnswerValue?.Trim() ?? city.Trim(),
                nationality = nationality,
                adults = int.TryParse(adultsStr, out var adultsParsed) ? adultsParsed : 1,
                students = int.TryParse(studentsStr, out var stuParsed) ? stuParsed : 0,
                children_6_12 = int.TryParse(children612Str, out var c612Parsed) ? c612Parsed : 0,
                children_under_4 = int.TryParse(childrenUnder4Str, out var cu4Parsed) ? cu4Parsed : 0,
                special_needs_count = int.TryParse(specialNeedsStr, out var snParsed) ? snParsed : 0,
                budgetAmount = double.TryParse(budgetStr, out var bParsed) ? bParsed : 5000.0,
                budgetType = budgetType,
                tier = tier,
                pacing = pacing,
                cities_to_visit = citiesList,
                inter_city_transport = interTransport,
                food_type = foodType,
                show_recipe_details = showRecipe,
                restrictions = restrictionsList,
                include_food = includeFood,
                guide_needed = guideNeeded,
                visited_before = visitedBefore,
                accessibility_needs = accessibilityNeeds,
                interests = interestsList
            };

            AIPlanResponseDto aiResponse;
            try
            {
                aiResponse = await _aiService.GeneratePlanAsync(travelData);
            }
            catch (Exception ex)
            {
                Console.WriteLine("🔥 AI ERROR: " + ex.Message);
                throw;
            }

            if (aiResponse == null || aiResponse.Days == null || !aiResponse.Days.Any())
                throw new Exception("AI Engine failed to generate a valid plan.");

            var planEntity = await MapAiResponseToPlanEntity(aiResponse, submission.UserId, city);
            var savedPlan = await _planRepository.CreatePlanAsync(planEntity);

            return MapPlanToDto(savedPlan);
        }


        // 3️⃣ الـ Mapping الاحترافي للـ Entity
        // داخل الـ MapAiResponseToPlanEntity
        private async Task<Plan> MapAiResponseToPlanEntity(AIPlanResponseDto aiResponse, string userId, string city)
        {
            var plan = new Plan
            {
                UserId = userId,
                City = city,

                TotalPriceEGP = aiResponse.TotalEstimatedCost,
                TotalEstimatedCost = aiResponse.TotalEstimatedCost,

                // 🔥 حل مشكلة totalBudget
                TotalBudget = aiResponse.TotalBudget > 0
                     ? (decimal)aiResponse.TotalBudget
                     : (decimal)aiResponse.TotalEstimatedCost,

                CreatedAt = DateTime.UtcNow,

                // 🔥 Fix Dates safely
                StartDate = aiResponse.Days != null && aiResponse.Days.Any()
                    ? DateTime.TryParse(aiResponse.Days.First().Date, out var start) ? start : DateTime.UtcNow
                    : DateTime.UtcNow,

                EndDate = aiResponse.Days != null && aiResponse.Days.Any()
                    ? DateTime.TryParse(aiResponse.Days.Last().Date, out var end) ? end : DateTime.UtcNow
                    : DateTime.UtcNow,

                BudgetLevel = aiResponse.TotalEstimatedCost > 20000 ? "High" : "Medium",
                PreferredTransport = "Uber",
                BudgetStatus = aiResponse.BudgetStatus,

                PlanDays = new List<PlanDay>(),
                BudgetBreakdown = new List<PlanBudgetItem>()
            };

            // 🔥 Budget Mapping + Fallback
            if (aiResponse.RecommendedBudget != null && aiResponse.RecommendedBudget.Any())
            {
                foreach (var item in aiResponse.RecommendedBudget)
                {
                    plan.BudgetBreakdown.Add(new PlanBudgetItem
                    {
                        Category = item.Key,
                        Limit = (decimal)item.Value,
                        Spent = aiResponse.ActualSpent != null && aiResponse.ActualSpent.ContainsKey(item.Key)
                            ? (decimal)aiResponse.ActualSpent[item.Key]
                            : (decimal)item.Value * 0.8m,
                        Status = aiResponse.BudgetStatus
                    });
                }
            }
            else
            {
                // 🔥 fallback لو AI مبعتش budget
                plan.BudgetBreakdown.Add(new PlanBudgetItem
                {
                    Category = "General",
                    Limit = aiResponse.TotalEstimatedCost,
                    Spent = aiResponse.TotalEstimatedCost * 0.8m,
                    Status = aiResponse.BudgetStatus
                });
            }

            // 🔥 Days Mapping
            foreach (var d in aiResponse.Days ?? new List<AIDayDto>())
            {
                var day = new PlanDay
                {
                    DayNumber = d.DayNumber,
                    Date = DateTime.TryParse(d.Date, out var dt) ? dt : DateTime.UtcNow,
                    City = d.City ?? city,

                    // 🍲 Food
                    MustTryFoodTitle = d.MustTryFood?.Title,
                    MustTryFoodDescription = d.MustTryFood?.Description,
                    MustTryFoodImage = d.MustTryFood?.ImageUrl,
                    MustTryFoodIngredients = d.MustTryFood?.Ingredients,
                    MustTryFoodInstructions = d.MustTryFood?.Instructions,
                    MustTryFoodCategory = d.MustTryFood?.Category,
                    MustTryFoodPriceRange = d.MustTryFood?.PriceRange,
                    MustTryFoodPriceState = d.MustTryFood?.PriceState  // ✅
                };

                // 🏨 Hotel (AI + fallback)
                // ✅ في الـ Hotel block:
                if (d.Hotel != null)
                {
                    day.HotelName = d.Hotel.Name;
                    day.HotelPrice = d.Hotel.Price;
                    day.HotelRating = d.Hotel.Rating;
                    day.HotelLocation = d.Hotel.Location;       // ✅ 
                    day.HotelMetroAccess = d.Hotel.MetroAccess; // ✅ 
                    day.HotelReviews = d.Hotel.Reviews;         // ✅ 
                    day.HotelRules = d.Hotel.Rules;
                    day.HotelImage = d.Hotel.ImageUrl; // ✅ احفظ الـ string كامل في الـ DB
                    day.HotelLatitude = d.Hotel.Latitude; // ✅
                    day.HotelLongitude = d.Hotel.Longitude; // ✅
                }
                else
                {
                    // fallback من DB
                    var hotelId = await GetRandomHotelIdAsync(city);
                    day.HotelId = hotelId;

                    day.HotelName = "Default Hotel";
                    day.HotelPrice = 1000;
                }

                // 🍽️ Meals (safe + fallback)
                // السطر 273-281 — استبدله بده
                // السطر 273-281 — استبدله بده
                day.Meals = (d.Meals != null && d.Meals.Any())
                    ? d.Meals.Select(m => new PlanMeal
                    {
                        Type = m.Type,
                        Name = m.Name,
                        Cost = m.Cost,
                        Description = m.Description,
                        Latitude = m.Latitude,
                        Longitude = m.Longitude,
                        ImageUrl = m.ImageUrl,
                        MinPrice = m.MinPrice,
                        MaxPrice = m.MaxPrice

                    }).ToList()
                    : GenerateMeals(city);

                // 🎯 Activities
                day.PlanActivities = (d.Activities ?? new List<AIActivityDto>())
                    .Select(a =>
                    {
                        var parsed = DateTime.TryParse(a.StartTime, out var st);
                        Console.WriteLine($"PlaceId => [{a.PlaceId}]");
                        return new PlanActivity
                        {
                            AIPlaceId = a.PlaceId,
                            Title = a.Title,
                            Description = a.Description,
                            Cost = a.Cost,
                            TransportCost = a.TransportCost,
                            Latitude = a.Lat,
                            Longitude = a.Lng,
                            ImageURL = (a.ImageURL != null && a.ImageURL.TrimStart().StartsWith("["))
                                ? System.Text.Json.JsonSerializer.Deserialize<List<string>>(a.ImageURL)?.FirstOrDefault() ?? ""
                                : a.ImageURL,

                            StartTime = a.StartTime,
                            EndTime = null,
                            RecommendedTransport = "Uber",
                            Category = "Activity"
                        };
                    }).ToList();

                plan.PlanDays.Add(day);
            }

            return plan;
        }



        #region Functions

        // ==========================================
        // استبدل الـ MapPlanToDto method بالكامل
        // ==========================================
        private PlanDto MapPlanToDto(Plan plan)
        {
            return new PlanDto
            {
                PlanId = plan.PlanId,
                Status = "success",
                City = plan.City,
                TotalEstimatedCost = plan.TotalEstimatedCost,
                TotalBudget = plan.TotalBudget,
                BudgetStatus = plan.BudgetStatus,

                RecommendedBudget = plan.BudgetBreakdown?
                    .ToDictionary(x => x.Category, x => x.Limit),

                ActualSpent = plan.BudgetBreakdown?
                    .ToDictionary(x => x.Category, x => x.Spent),

                Days = plan.PlanDays.Select(day => new PlanDayDto
                {
                    DayNumber = day.DayNumber,
                    Date = day.Date.ToString("yyyy-MM-dd"),   // ✅ string بصيغة FastAPI
                    City = day.City,

                    Hotel = day.HotelName != null ? new HotelOutputDto
                    {
                        Name = day.HotelName,
                        Price = (double)(day.HotelPrice ?? 0),
                        Images = ParseHotelImages(day.HotelImage),
                        Rating = (double)(day.HotelRating ?? 0),
                        Location = day.HotelLocation,
                        MetroAccess = day.HotelMetroAccess ?? "",
                        Reviews = day.HotelReviews ?? 0,
                        Rules = day.HotelRules ?? "",
                        Latitude = day.HotelLatitude,
                        Longitude = day.HotelLongitude,
                    } : null,

                    MustTryFood = day.MustTryFoodTitle != null ? new FoodDto
                    {
                        Title = day.MustTryFoodTitle ?? "",
                        Description = day.MustTryFoodDescription ?? "",
                        ImageUrl = day.MustTryFoodImage,
                        Category = day.MustTryFoodCategory,
                        Ingredients = day.MustTryFoodIngredients ?? "",
                        Instructions = day.MustTryFoodInstructions ?? "",
                        PriceRange = day.MustTryFoodPriceRange ?? "",
                        PriceState = day.MustTryFoodPriceState ?? ""
                    } : null,

                    Meals = day.Meals.Select(m => new MealDto
                    {
                        Type = m.Type,
                        Name = m.Name,
                        Cost = m.Cost,
                        Description = m.Description,

                        Latitude = m.Latitude,
                        Longitude = m.Longitude,

                        ImageUrl = m.ImageUrl,
                        MinPrice = m.MinPrice,
                        MaxPrice = m.MaxPrice

                    }).ToList(),

                    Activities = day.PlanActivities.Select(a => new PlanActivityDto
                    {
                        AIPlaceId = a.AIPlaceId,
                        Title = a.Title,
                        Description = a.Description,
                        Cost = a.Cost ?? 0,
                        TransportCost = a.TransportCost ?? 0,
                        Lat = a.Latitude ?? 0,
                        Lng = a.Longitude ?? 0,
                        ImageUrl = a.ImageURL,
                        StartTime = a.StartTime
                    }).ToList()

                }).ToList()
            };
        }

        private List<PlanMeal> GenerateMeals(string city)
        {
            return new List<PlanMeal>
    {
        new PlanMeal
        {
            Type = "Lunch",
            Name = "Local Restaurant",
            Cost = 150
        },
        new PlanMeal
        {
            Type = "Dinner",
            Name = "Fine Dining",
            Cost = 300
        }
    };
        }
        private async Task<int?> GetRandomHotelIdAsync(string city)
        {
            return await _hotelRepository.GetRandomHotelIdAsync(city);
        }
        private List<string> ParseHotelImages(string? rawImages)
        {
            if (string.IsNullOrWhiteSpace(rawImages))
                return new List<string>();

            try
            {
                // لو الداتا بتبدأ بـ [ يبقى ده JSON Array محتاج يفك
                if (rawImages.Trim().StartsWith("["))
                {
                    return JsonSerializer.Deserialize<List<string>>(rawImages) ?? new List<string>();
                }

                // لو هي صورة واحدة عادية، حطها في لستة ورجعها
                return new List<string> { rawImages };
            }
            catch
            {
                // لو حصل أي مشكلة في الـ Parse، رجع الداتا زي ما هي في لستة عشان السيستم ميعطلش
                return new List<string> { rawImages };
            }
        }
        private (string Title, string Description) GenerateMustTryFood(string city)
        {
            return ("Koshari", "Traditional Egyptian dish");
        }
        #endregion


        public async Task<bool> DeletePlanAsync(int planId, string userId)
        {

            return await _planRepository.DeletePlanAsync(planId, userId);
        }
        public async Task<PlanDto?> GetUserCurrentPlanAsync(string userId)
        {
            var plan = await _planRepository.GetCurrentPlanAsync(userId);
            if (plan == null) return null;
            return MapPlanToDto(plan);   // ✅ بدل _mapper.Map<PlanDto>(plan)
        }

        public async Task<IEnumerable<PlanDto>> GetUserHistoryAsync(string userId)
        {
            var plans = await _planRepository.GetUserPlansHistoryAsync(userId);
            return plans.Select(p => MapPlanToDto(p)).ToList();   // ✅ بدل _mapper
        }

        public async Task<PlanDto?> GetPlanDetailsAsync(int planId, string userId)
        {
            var plan = await _planRepository.GetPlanByIdAsync(planId, userId);
            if (plan == null) return null;
            return MapPlanToDto(plan);   // ✅ بدل _mapper
        }



    }
}

