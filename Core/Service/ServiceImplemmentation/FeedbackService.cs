using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Contracts.Repo;
using DomainLayer.Models.FeedbackModule;
using ServiceAbstraction.Services;
using Shared.DTOS.APIFormsDTOs;

namespace Service.ServiceImplemmentation
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IAIService _aiService;
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackService(IAIService aiService, IFeedbackRepository feedbackRepository)
        {
            _aiService = aiService;
            _feedbackRepository = feedbackRepository;
        }

        public async Task<string> SubmitFeedbackAsync(string userId, SubmitFeedbackDto dto)
        {
            // 1. احفظ في الـ DB
            var entities = dto.Feedback.Select(f => new UserFeedback
            {
                UserId = userId,
                PlaceId = f.PlaceId,
                Rating = f.Rating,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            await _feedbackRepository.SaveFeedbackAsync(entities);

            // 2. ابعت للـ AI
            var result = await _aiService.SendFeedbackAsync(userId, dto);

            return result;
        }
    }
}
