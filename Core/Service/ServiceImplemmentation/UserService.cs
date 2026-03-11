using DomainLayer.Contracts.Repo;
using ServiceAbstraction.Services;
using Shared.DTOS.APIFormsDTOs;
using Shared.DTOS.UserProfileDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service.ServiceImplemmentation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<UserProfileDto?> GetProfileAsync(string userId)
        {
            var user = await _userRepo.GetUserProfileAsync(userId);
            if (user == null) return null;

            // محاولة استخراج الاهتمام من آخر نتيجة AI مخزنة
            string? extractedInterest = null;
            var lastResult = user.FormSubmissions.LastOrDefault()?.AIResults.LastOrDefault();

            if (lastResult != null && !string.IsNullOrEmpty(lastResult.ResultJson))
            {
                try
                {
                    // هنا بنحول الـ JSON لنوع مجهول (Anonymous) عشان نسحب منه معلومة
                    // افترضنا إن الـ JSON جواه حقل اسمه "Category" أو "Interests"
                    var jsonDoc = JsonDocument.Parse(lastResult.ResultJson);
                    if (jsonDoc.RootElement.TryGetProperty("Category", out var categoryProp))
                    {
                        extractedInterest = categoryProp.GetString();
                    }
                }
                catch { /* في حالة الـ JSON مش سليم مش عايزين الـ App يقع */ }
            }

            return new UserProfileDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? "",
                Nationality = user.Nationality,
                PreferredLanguage = user.PreferredLanguage,
                ProfileImageUrl = user.ProfileImageUrl,
                TotalPlans = user.Plans.Count,
                // عرض الاهتمام المستخرج أو قيمة افتراضية
               // TopInterest = extractedInterest ?? "Explore Egypt"
            };
        }

        public async Task<bool> UpdateProfileAsync(string userId, UpdateProfileDto updateDto)
        {
            var user = await _userRepo.GetUserProfileAsync(userId);
            if (user == null) return false;

            // لو اليوزر باعت صورة جديدة
            if (updateDto.ProfileImage != null && updateDto.ProfileImage.Length > 0)
            {
                // 1. تحديد مكان الحفظ (wwwroot/images/profiles)
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "profiles");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                // 2. عمل اسم فريد للصورة عشان الأسماء ما تخبطش في بعضها
                var fileName = $"{userId}_{Guid.NewGuid()}{Path.GetExtension(updateDto.ProfileImage.FileName)}";
                var filePath = Path.Combine(folderPath, fileName);

                // 3. حفظ الملف
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await updateDto.ProfileImage.CopyToAsync(stream);
                }

                // 4. تخزين المسار في اليوزر (عشان يرجع كـ URL للموبايل)
                user.ProfileImageUrl = $"/images/profiles/{fileName}";
            }

            user.FullName = updateDto.FullName;
            user.Nationality = updateDto.Nationality;
            user.PreferredLanguage = updateDto.PreferredLanguage;

            return await _userRepo.UpdateProfileAsync(user);
        }
    }
}
