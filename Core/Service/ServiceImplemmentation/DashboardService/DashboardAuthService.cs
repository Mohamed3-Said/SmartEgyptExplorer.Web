using DomainLayer.Contracts.Repo.DashboardRepo;
using DomainLayer.DashboardModule;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using ServiceAbstraction.Services.DashboardServices;
using Shared.DTOS.DashboardDTOs.DashboardAuthDTOs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace Service.ServiceImplemmentation.DashboardService
{
    public class DashboardAuthService : IDashboardAuthService
    {
        private readonly IDashboardAuthRepo _repo;
        private readonly IConfiguration _config;
        private readonly DashEmailService _emailService;

        public DashboardAuthService(IDashboardAuthRepo repo, IConfiguration config , DashEmailService emailService)
        {
            _repo = repo;
            _config = config;
           _emailService = emailService;
        }

        public async Task<RegisterStep1ResponseDto> RegisterStep1Async(
            RegisterStep1Dto dto, string documentUrl, string? photoUrl)
        {
            // تأكد إن الإيميل مش موجود
            var existing = await _repo.GetByEmailAsync(dto.Email);
            if (existing != null)
                throw new Exception("Email already registered.");

            var user = new DashboardUser
            {
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                BusinessType = dto.BusinessType,
                Title = dto.Title,
                City = dto.City,
                DocumentUrl = documentUrl,
                PhotoUrl = photoUrl,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            await _repo.CreateAsync(user);

            return new RegisterStep1ResponseDto
            {
                DashboardUserId = user.DashboardUserId,
                Message = "Step 1 completed. Please complete your registration."
            };
        }

        public async Task CompleteRegistrationAsync(RegisterStep2Dto dto)
        {
            var user = await _repo.GetByIdAsync(dto.DashboardUserId);
            if (user == null) throw new Exception("User not found.");

            user.Location = dto.Location;
            user.Longitude = dto.Longitude;
            user.Latitude = dto.Latitude;
            // ✅ Restaurant specific
            if (user.BusinessType == "Restaurant")
            {
                user.Category = dto.Category;
                user.RestaurantImageUrl = dto.RestaurantImageUrl;
                user.MinPrice = dto.MinPrice;
                user.MaxPrice = dto.MaxPrice;
            }

            // TourGuide specific
            // 🔥 هنا بقى الحل
            var normalized = user.BusinessType.Replace(" ", "");

            if (normalized == "TourGuide")
            {
                user.Name = dto.Name;
                user.Age = dto.Age;
                user.Languages = dto.Languages;
                user.PhoneNumber = dto.PhoneNumber;
            }

            await _repo.UpdateAsync(user);
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _repo.GetByEmailAsync(dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid email or password.");

            // 🔥 Deleted / Hidden
            if (user.IsHidden)
                throw new Exception("This account has been deleted.");

            // 🔥 Pending
            if (user.Status == "Pending")
                throw new Exception("Your account is still under review.");

            // 🔥 Rejected
            if (user.Status == "Rejected")
                throw new Exception("Your application has been rejected.");

            // ✅ Approved 
            var token = GenerateJwt(user);

            return new LoginResponseDto
            {
                Token = token,
                Role = user.Role,
                BusinessType = user.BusinessType,
                Status = user.Status
            };
        }
        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _repo.GetByEmailAsync(email);
            if (user == null) throw new Exception("Email not found.");

            var code = new Random().Next(100000, 999999).ToString();
            user.ResetCode = code;
            user.ResetCodeExpiry = DateTime.UtcNow.AddMinutes(10);
            await _repo.UpdateAsync(user);

            await _emailService.SendEmailAsync(
                email,
                "Password Reset Code",
                $"Your reset code is: {code}"
            );
        }

        public async Task<bool> VerifyCodeAsync(VerifyCodeDto dto)
        {
            var user = await _repo.GetByEmailAsync(dto.Email);
            if (user == null) return false;

            return user.ResetCode == dto.Code &&
                   user.ResetCodeExpiry > DateTime.UtcNow;
        }

        public async Task ResetPasswordAsync(ResetPasswordDto dto)
        {
            if (dto.NewPassword != dto.ConfirmPassword)
                throw new Exception("Passwords do not match.");

            var user = await _repo.GetByEmailAsync(dto.Email);
            if (user == null) throw new Exception("User not found.");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.ResetCode = null;
            user.ResetCodeExpiry = null;
            await _repo.UpdateAsync(user);
        }

        private string GenerateJwt(DashboardUser user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!));

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.DashboardUserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("BusinessType", user.BusinessType),
                new Claim("Status", user.Status)
            };

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(
                    int.Parse(_config["JwtSettings:ExpiryInDays"]!)),
                signingCredentials: new SigningCredentials(
                    key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
