using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Exceptions.UserExceptions;
using DomainLayer.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Bcpg;
using ServiceAbstraction;
using Shared.DTOS.IdentityModuleDtos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AuthService(UserManager<AppUser> _userManager,
        IConfiguration _configuration, IRefreshTokenRepository _repository ,
        IEmailService _emailService , IPasswordResetCodeRepository _passwordResetRepo) : IAuthService
    {
        private readonly int _codeExpiryMinutes =
               _configuration.GetValue<int>("ResetPasswordOptions:CodeExpiryMinutes");

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            //MApping RegisterDto to AppUser :
            var user = new AppUser
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                FullName = registerDto.FullName,
                PhoneNumber = registerDto.PhoneNumber,
                Nationality = registerDto.Nationality,
                PreferredLanguage = registerDto.PreferredLanguage,
                ProfileImageUrl = "default-profile.png", // added here (not from DTO)
                CreatedAt = DateTime.UtcNow
            };
            var Result = await _userManager.CreateAsync(user, registerDto.Password);
            if (Result.Succeeded)
            {
                var allowedRoles = new[] { "Tourist", "TourGuide" , "Admin" };
                if (!allowedRoles.Contains(registerDto.Role))
                    throw new BadRequestException("Invalid role selected!");
                // Add role to user
                var role = string.IsNullOrWhiteSpace(registerDto.Role) ? "Tourist" : registerDto.Role;
                await _userManager.AddToRoleAsync(user, role);
                var expiresAt = DateTime.UtcNow.AddDays(7);
                var refreshToken = await GenerateRefreshTokenAsync(user);
                return new AuthResponseDto
                {
                    Email = user.Email,
                    FullName = user.FullName,
                    Role = registerDto.Role,
                    Token = await CreateTokenAsync(user),
                    RefreshToken = refreshToken,
                    ExpiresAt = expiresAt
                };
            }
            else
            {
                //errors from Result and Add to Exception:
                var errors = Result.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(errors);
            }
        }
        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            //1-Check of Email :
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null)
                throw new UserNotFoundException(loginDto.Email);
            //-2Check of Password :
            var PasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            var roles = await _userManager.GetRolesAsync(user);
            var expiresAt = DateTime.UtcNow.AddDays(7);
            var refreshToken = await GenerateRefreshTokenAsync(user);
            if (PasswordValid)
            {
                return new AuthResponseDto
                {
                    Email = user.Email!,
                    FullName = user.FullName,
                    Role = roles.FirstOrDefault() ?? "Tourist",
                    Token = await CreateTokenAsync(user),
                    RefreshToken = refreshToken,
                    ExpiresAt = expiresAt
                };
            }
            else
                throw new UnAuthorizedException();
        }
        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var existingToken = await _repository.GetByTokenAsync(refreshToken);
            if (existingToken is null || existingToken.IsRevoked || existingToken.ExpiresAt < DateTime.UtcNow)
                throw new UnAuthorizedException("Invalid or expired refresh token!");
            // Get user
            var user = await _userManager.FindByIdAsync(existingToken.UserId);
            if (user is null)
                throw new UserRefreshNotFoundExceptions(existingToken.UserId);
            // Revoke existing token
            existingToken.IsRevoked = true;

            // Create new refresh token
            var newRefreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };
            await _repository.AddAsync(newRefreshToken);
            await _repository.SaveChangesAsync();

            // Return new tokens
            return new AuthResponseDto
            {
                Email = user.Email!,
                FullName = user.FullName,
                Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "Tourist",
                Token = await CreateTokenAsync(user),
                RefreshToken = newRefreshToken.Token,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var existingToken = await _repository.GetByTokenAsync(refreshToken);
            if (existingToken is null)
                throw new UnAuthorizedException("Invalid refresh token!");
            if (existingToken.IsRevoked)
                throw new BadRequestException(" Token already revoked!");
            // Revoke it
            existingToken.IsRevoked = true;
            await _repository.SaveChangesAsync();
        }
        public async Task ForgetPasswordAsync(ForgetPasswordDto forgetPasswordDto)
        {
            //1-check of email :
            var user = await _userManager.FindByEmailAsync(forgetPasswordDto.Email);
            if (user is null)
                throw new UserNotFoundException(forgetPasswordDto.Email);
            //2- generate an 6-digit code : 
            var random = new Random();
            var code = random.Next(100000, 999999).ToString();
            //3-Store in PasswordResetCodes :
            var resetCode = new PasswordResetCode
            {
                UserId = user.Id,
                Code = code,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                IsUsed = false
            };
            await _passwordResetRepo.AddAsync(resetCode);
            await _passwordResetRepo.SaveChangesAsync();

            //4-Prepare the email
            var emailMessage = new EmailMessageDto
            {
                To = user.Email!,
                Subject = "Your Smart Explorer password reset code",
                Body = $"<p>Hello {user.FullName},</p>" +
             $"<p>Your password reset code is: <b>{code}</b></p>" +
             $"<p>This code will expire in 10 minutes.</p>"
            };
            //5- Send by EmailService:
            await _emailService.SendEmailAsync(emailMessage);
        }
        public async Task VerifyResetCodeAsync(VerifyResetCodeDto verifyResetCodeDto)
        {
            var user = await _userManager.FindByEmailAsync(verifyResetCodeDto.Email);
            if (user is null)
                throw new UserNotFoundException(verifyResetCodeDto.Email);
            var validCode = await _passwordResetRepo.GetValidCodeAsync(verifyResetCodeDto.Email, verifyResetCodeDto.Code);
            if (validCode is null)
                throw new BadRequestException("Invalid or expired reset code.");
        }
        public async Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            //1-check of email :
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user is null)
                throw new UserNotFoundException(resetPasswordDto.Email);
            //2-Get the reset code from the table
            var validCode = await _passwordResetRepo.GetValidCodeAsync(resetPasswordDto.Email, resetPasswordDto.Code);
            if (validCode is null)
                throw new BadRequestException("Invalid or expired reset code.");
            //3-GeneratePasswordResetToken
            var identityResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            //4- changed his password
            var result = await _userManager.ResetPasswordAsync(user, identityResetToken,resetPasswordDto.NewPassword);
            if (!result.Succeeded)
                throw new BadRequestException(result.Errors.Select(e => e.Description).ToList());
            //5-teach the reset code as a user
            validCode.IsUsed = true;
            await _passwordResetRepo.SaveChangesAsync();
        }


        #region Create Token Method :
        private async Task<string> CreateTokenAsync(AppUser user)
        {
            //1-create claims : 
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName!),
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim(ClaimTypes.NameIdentifier,user.Id!)
            };
            //2-Get user roles :
            var Roles = await _userManager.GetRolesAsync(user);
            foreach (var role in Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
                claims.Add(new Claim("Role", role));

            }
            //3-Create secrte key :
            var secretKey = _configuration.GetSection("JWTOptions")["SecretKey"];
            //4-convert secrte key to byte array :
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            //5-Create signing credentials :
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //6-Create token  :
            var Token = new JwtSecurityToken
            (
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials,
                issuer: _configuration.GetSection("JWTOptions")["Issuer"],
                audience: _configuration.GetSection("JWTOptions")["Audience"]
            );
            //7-Return token :
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
        #endregion

        #region Generate Refresh Token
        private async Task<string> GenerateRefreshTokenAsync(AppUser user)
        {
            var refreshExpiryDays = int.Parse(_configuration["JWTOptions:RefreshTokenExpiryDays"] ?? "7");
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshExpiryDays),
                IsRevoked = false
            };
            await _repository.AddAsync(refreshToken);
            await _repository.SaveChangesAsync();
            return refreshToken.Token;
        }


        #endregion


    }
}
