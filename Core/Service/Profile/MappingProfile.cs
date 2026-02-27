using AutoMapper;
using DomainLayer.Models.Models.PlanModule;
using DomainLayer.Models.Models.Remaining_Modules;
using DomainLayer.Models.PlanModule;
using DomainLayer.Models.Remaining_Modules;
using Shared.DTOS.APIFormsDTOs;
using Shared.DTOS.APIFormsDTOs.AIDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Service.Profile
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            // Plan Mapping
            CreateMap<Plan, PlanDto>()
                .ForMember(dest => dest.Days,
                    opt => opt.MapFrom(src => src.PlanDays));

            CreateMap<PlanDay, PlanDayDto>()
                .ForMember(dest => dest.Activities,
                    opt => opt.MapFrom(src => src.PlanActivities));

            CreateMap<PlanActivity, PlanActivityDto>();
            CreateMap<PlanBudgetItem, BudgetBreakdownDto>();

            // Voice Translation Mapping
            CreateMap<VoiceTranslationSession, VoiceTranslationSessionDto>();
            CreateMap<VoiceTranslationMessage, VoiceTranslationMessageInputDto>()
                .ForMember(dest => dest.MessageId, opt => opt.MapFrom(src => src.VoiceTranslationMessageId));
        }
    }
}
