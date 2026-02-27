
using DomainLayer.Contracts;
using DomainLayer.Contracts.Repo;
using DomainLayer.Engines;
using DomainLayer.Helpers;
using DomainLayer.Models.IdentityModule;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Pqc.Crypto.Frodo;
using Persistence.Data.configurations;
using Persistence.Data.Repositories;
using Persistence.Data.Repositories.Repo;
using Service;
using Service.Profile;
using Service.ServiceImplemmentation;
using ServiceAbstraction;
using ServiceAbstraction.Services;
using SmartEgyptExplorer.CustomExceptionMiddelWare;
using System.Text;

namespace SmartEgyptExplorer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add services to the container.
            #region Identity Services
            builder.Services.AddDbContext<SmartIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                            .AddEntityFrameworkStores<SmartIdentityDbContext>()
                            .AddDefaultTokenProviders();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IPasswordResetCodeRepository, PasswordResetCodeRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            #endregion



            #region Add Application Services
            builder.Services.AddScoped<IVoiceTranslationRepository, VoiceTranslationRepository>();
            builder.Services.AddScoped<IAIResultRepository, AIResultRepository>();
            builder.Services.AddScoped<IUserFormRepository, UserFormRepository>(); 
            builder.Services.AddScoped<IPlanRepository, PlanRepository>();


            builder.Services.AddScoped<IFormService, FormService>();
            builder.Services.AddScoped<IVoiceTranslationService, VoiceTranslationService>();
            builder.Services.AddScoped<IAIService, AIService>();
            builder.Services.AddHttpClient<IAIService, AIService>(client => {
                client.BaseAddress = new Uri("http://127.0.0.1:8000/");
            });
            builder.Services.AddScoped<LogisticsEngine>();
            builder.Services.AddScoped<FoodEngine>();
            builder.Services.AddScoped<AttractionCsvModel>();
            builder.Services.AddScoped<RecommenderEngine>();
            #endregion

            builder.Services.AddDbContext<SmartEgyptDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            #endregion

            #region Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });
            #endregion

            #region JWT Bearer Authentication Middleware
            builder.Services.AddAuthentication(optins =>
            {
                optins.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                optins.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWTOptions:Issuer"],
                    ValidAudience = builder.Configuration["JWTOptions:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTOptions:SecretKey"]!))
                };
            });

            #endregion

            var app = builder.Build();
            #region Role Seeding 
            using var scope = app.Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roles = new[] { "Tourist", "TourGuide" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
            #endregion


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<CustomExceptionHandlerMiddelWare>();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
