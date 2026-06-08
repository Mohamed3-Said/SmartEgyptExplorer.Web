using AutoMapper;
using DomainLayer.Models.InfoBankModule;
using DomainLayer.Models.Models.PlanModule;
using DomainLayer.Models.Models.Remaining_Modules;
using DomainLayer.Models.PlanModule;
using DomainLayer.Models.Remaining_Modules;
using Shared.DTOS.APIFormsDTOs;
using Shared.DTOS.APIFormsDTOs.AIDTOs;
using Shared.DTOS.APIFormsDTOs.AIDTOs.DetailsDTOS;
using Shared.DTOS.InfoBankDTOs;
using Shared.DTOS.VoiceTranslationDTO.GetVoiceDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace Service.Profile
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            #region Plan Mapping
            CreateMap<Plan, PlanDto>()
                .ForMember(dest => dest.PlanId, opt => opt.MapFrom(src => src.PlanId))
                .ForMember(dest => dest.Days,
                    opt => opt.MapFrom(src => src.PlanDays))
                .ForMember(dest => dest.RecommendedBudget, opt => opt.MapFrom(src =>
                    src.BudgetBreakdown.ToDictionary(x => x.Category, x => x.Limit)))
                .ForMember(dest => dest.ActualSpent, opt => opt.MapFrom(src =>
                    src.BudgetBreakdown.ToDictionary(x => x.Category, x => x.Spent)));

            CreateMap<PlanDay, PlanDayDto>()
                .ForMember(dest => dest.Activities,
                    opt => opt.MapFrom(src => src.PlanActivities))
                .ForMember(dest => dest.Meals,
                    opt => opt.MapFrom(src => src.Meals))
                .ForMember(dest => dest.Hotel, opt => opt.MapFrom(src =>
                    src.HotelName == null ? null : new HotelOutputDto
                    {
                        Name = src.HotelName,
                        Price = src.HotelPrice ?? 0,
                        Images = ImageHelper.ParseHotelImages(src.HotelImage),
                        Rating = src.HotelRating ?? 0,
                        Location = src.HotelLocation,
                        MetroAccess = src.HotelMetroAccess ?? "",
                        Reviews = src.HotelReviews ?? 0,
                        Rules = src.HotelRules ?? "",
                        Latitude = src.HotelLatitude,
                        Longitude = src.HotelLongitude
                    }))
                .ForMember(dest => dest.MustTryFood, opt => opt.MapFrom(src =>
        src.MustTryFoodTitle == null ? null : new FoodDto
        {
            Title = src.MustTryFoodTitle,
            Description = src.MustTryFoodDescription ?? "",
            ImageUrl = src.MustTryFoodImage,
            Category = src.MustTryFoodCategory,
            Ingredients = src.MustTryFoodIngredients ?? "",
            Instructions = src.MustTryFoodInstructions ?? "",
            PriceRange = src.MustTryFoodPriceRange ?? "",
            PriceState = src.MustTryFoodPriceState ?? ""
        }));

            CreateMap<PlanActivity, PlanActivityDto>()
                .ForMember(dest => dest.Lat, opt => opt.MapFrom(src => src.Latitude ?? 0))
                .ForMember(dest => dest.Lng, opt => opt.MapFrom(src => src.Longitude ?? 0))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageURL));
            CreateMap<PlanBudgetItem, BudgetBreakdownDto>();
            CreateMap<PlanMeal, MealDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Cost))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.MinPrice, opt => opt.MapFrom(src => src.MinPrice))
                .ForMember(dest => dest.MaxPrice, opt => opt.MapFrom(src => src.MaxPrice));

            #endregion

            #region Voice Translation Mapping
            CreateMap<VoiceTranslationSession, VoiceTranslationSessionDto>();
            CreateMap<VoiceTranslationMessage, VoiceTranslationMessageInputDto>()
                .ForMember(dest => dest.MessageId, opt => opt.MapFrom(src => src.VoiceTranslationMessageId));

            CreateMap<VoiceTranslationMessage, VoiceTranslationMessageDto>();
            CreateMap<VoiceTranslationSession, VoiceTranslationSessionGetDto>();
            #endregion

            #region InfoBankMapping:

            //Attractions Details :
            CreateMap<AttractionInfo, AttractionDto>()
                .ForMember(d => d.Description, o => o.MapFrom(s =>
                   s.Description != null ? s.Description.Replace("\n", " ") : null))
                .ForMember(d => d.FreeEntryPolicy, o => o.MapFrom(s =>
                   s.Description != null ? s.FreeEntryPolicy.Replace("\n", " ") : null));

            //Attraction Summary :
            CreateMap<AttractionInfo, AttractionSummaryDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.AttractionInfoId))
                .ForMember(d => d.Rating, o => o.MapFrom(s => s.AverageRating))
                .ForMember(d => d.Duration, o => o.MapFrom(s => s.ExploreDurationMin ?? 0))
                .ForMember(d => d.PriceFrom, o => o.MapFrom(s =>
                    Math.Min(s.EgyptianAdultPrice,
                    Math.Min(s.ArabAdultPrice, s.ForeignerAdultPrice))
                ));
            //Hotel Summary
            CreateMap<HotelInfo, HotelSummaryDto>()
                .ForMember(d => d.FirstImageUrl, o => o.MapFrom(s =>
                    JsonHelper.GetFirstImage(s.Images)));

            //Hotel Details
            CreateMap<HotelInfo, HotelDetailsDto>()
                .ForMember(d => d.Images, o => o.MapFrom(s =>
                    JsonHelper.DeserializeList(s.Images)))
                .ForMember(d => d.HouseRules, o => o.MapFrom(s =>
                    JsonHelper.DeserializeComplexList(s.HouseRules)))
                .ForMember(d => d.Availability, o => o.MapFrom(s =>
                    JsonHelper.DeserializeComplexList(s.Availability)))
                .ForMember(d => d.PopularFacilities, o => o.MapFrom(s =>
                   JsonHelper.DeserializeList(s.PopularFacilities)))
               .ForMember(d => d.LanguagesSpoken, o => o.MapFrom(s =>
                   JsonHelper.DeserializeList(s.LanguagesSpoken)))
               .ForMember(d => d.Description, o => o.MapFrom(s =>
                   s.Description != null ? s.Description.Replace("\n", " ") : null));


            // Restaurants
            CreateMap<RestaurantInfo, RestaurantSummaryDto>();

            // Food Recipes
            CreateMap<FoodRecipe, FoodRecipeSummaryDto>();
            CreateMap<FoodRecipe, FoodRecipeDetailsDto>();
            #endregion


        }
        public static class JsonHelper
        {
            public static List<string> DeserializeList(string? json)
            {
                if (string.IsNullOrWhiteSpace(json))
                    return new List<string>();

                try
                {
                    List<string> list;

                    if (json.Trim().StartsWith("["))
                    {
                        list = JsonSerializer.Deserialize<List<string>>(json)
                               ?? new List<string>();
                    }
                    else if (json.Contains(","))
                    {
                        list = json.Split(',').Select(x => x.Trim()).ToList();
                    }
                    else
                    {
                        list = new List<string> { json };
                    }

                    return list
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .Where(x => !x.Contains("See all"))
                        .Distinct()
                        .ToList();
                }
                catch
                {
                    return new List<string>();
                }
            }
            public static List<string> DeserializeComplexList(string? json)
            {
                if (string.IsNullOrWhiteSpace(json))
                    return new List<string>();

                try
                {
                    var list = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(json);

                    return list?
                        .SelectMany(dict => dict.Values)   // ياخد كل values
                        .Select(v => v?.ToString())
                        .Where(v => !string.IsNullOrWhiteSpace(v))
                        .ToList()
                        ?? new List<string>();
                }
                catch
                {
                    return new List<string>();
                }
            }
            public static string? GetFirstImage(string? json)
            {
                var list = DeserializeList(json);

                return list.FirstOrDefault()
                    ?? "https://via.placeholder.com/300";
            }
        }
        public static class ImageHelper
        {
            public static List<string> ParseHotelImages(string? rawImages)
            {
                if (string.IsNullOrWhiteSpace(rawImages))
                    return new List<string>();

                try
                {
                    if (rawImages.Trim().StartsWith("["))
                    {
                        return JsonSerializer.Deserialize<List<string>>(rawImages) ?? new List<string>();
                    }

                    return new List<string> { rawImages };
                }
                catch
                {
                    return new List<string> { rawImages };
                }
            }
        }
    }
 }
