using DomainLayer.Contracts;
using DomainLayer.Contracts;
using DomainLayer.Contracts.Repo;
using DomainLayer.Contracts.Repo;
using DomainLayer.Contracts.Repo.IMobileRepo;
using DomainLayer.Contracts.Repo.InfoBankRepo;
using DomainLayer.Engines;
using DomainLayer.Helpers;
using DomainLayer.Models.IdentityModule;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.Data.configurations;
using Persistence.Data.Repositories;
using Persistence.Data.Repositories.Repo;
using Persistence.Data.Repositories.Repo.InfoBankRepository;
using Persistence.Data.Repositories.Repo.MobileRepository;
using Persistence.Data.Seeders;
using Service;
using Service.Profile;
using Service.ServiceImplementation;
using Service.ServiceImplemmentation;
using Service.ServiceImplemmentation.InfoBankService;
using Service.ServiceImplemmentation.MobileService;
using ServiceAbstraction;
using ServiceAbstraction.Services;
using ServiceAbstraction.Services.IMobileService;
using ServiceAbstraction.Services.InfoBankIService;
using SmartEgyptExplorer.CustomExceptionMiddelWare;
using System.Text;

namespace SmartEgyptExplorer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Database Context
            builder.Services.AddDbContext<SmartEgyptDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 2. Identity Configuration
            builder.Services.AddIdentity<AppUser, IdentityRole>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<SmartEgyptDbContext>()
            .AddDefaultTokenProviders();

            // 3. Register Repositories (Scoped)
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            builder.Services.AddScoped<IPasswordResetCodeRepository, PasswordResetCodeRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IVoiceTranslationRepository, VoiceTranslationRepository>();
            builder.Services.AddScoped<IAIResultRepository, AIResultRepository>();
            builder.Services.AddScoped<IUserFormRepository, UserFormRepository>();
            builder.Services.AddScoped<IPlanRepository, PlanRepository>();
            builder.Services.AddScoped<IHotelRepository, HotelRepository>();
            //Data Seeder
            builder.Services.AddScoped<IDataSeederRepo, DataSeederRepo>();
            builder.Services.AddScoped<IDataSeederService, DataSeederService>();
            // Feedback Service :
            builder.Services.AddScoped<IFeedbackService, FeedbackService>();
            builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();

            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 100 * 1024 * 1024; // 100MB
            });
            // InfoBank Registration :
            builder.Services.AddScoped<IAttractionRepo, AttractionRepo>();
            builder.Services.AddScoped<IAttractionService, AttractionService>();
            builder.Services.AddScoped<IHotelRepo, HotelRepo>();
            builder.Services.AddScoped<IHotelService, HotelService>();
            builder.Services.AddScoped<IRestaurantRepo, RestaurantRepo>();
            builder.Services.AddScoped<IRestaurantService, RestaurantService>();
            builder.Services.AddScoped<IFoodRecipeRepo, FoodRecipeRepo>();
            builder.Services.AddScoped<IFoodRecipeService, FoodRecipeService>();
            // 4. Register Services & HttpClient
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IFormService, FormService>();
            builder.Services.AddScoped<IVoiceTranslationService, VoiceTranslationService>();

            // Mobile Tour Guide Services :
            builder.Services.AddScoped<ITourGuideRepo, TourGuideRepo>();
            builder.Services.AddScoped<ITourGuideService, TourGuideService>();
            //Booking & Tickets Services :
            builder.Services.AddScoped<ITicketRepo, TicketRepo>();
            builder.Services.AddScoped<ITicketService, TicketService>();
            #region AI Service Registration with HttpClient
            // AI Planner
            builder.Services.AddHttpClient("AIPlannerClient", client =>
            {
                client.BaseAddress = new Uri("https://gizmo-residency-upscale.ngrok-free.dev/");
                client.Timeout = TimeSpan.FromMinutes(3);
                client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "true");
            });

            // AI Translator
            builder.Services.AddHttpClient("AITranslatorClient", client =>
            {
                client.BaseAddress = new Uri("https://proliferous-nontypically-michelina.ngrok-free.dev/");
                client.Timeout = TimeSpan.FromMinutes(10);
                client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "true");
            });

            builder.Services.AddScoped<IAIService, AIService>();
            #endregion

            // 5. Engines & Models
            builder.Services.AddScoped<LogisticsEngine>();
            builder.Services.AddScoped<FoodEngine>();
            builder.Services.AddScoped<AttractionCsvModel>();
            builder.Services.AddScoped<RecommenderEngine>();

            // 6. AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // 7. Controllers & API Documentation (Swagger)
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Smart Egypt API", Version = "v1" });
            });

            // 8. CORS Policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // 9. JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWTOptions:Issuer"],
                    ValidAudience = builder.Configuration["JWTOptions:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JWTOptions:SecretKey"] ?? "YourSuperSecretKeyGoesHere123!"))
                };
            });

            var app = builder.Build();

            // 10. Automatic Role Seeding
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                string[] roles = { "Tourist", "TourGuide" , "Admin" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // --- Middleware Pipeline ---

            // Handling Exceptions first
            app.UseMiddleware<CustomExceptionHandlerMiddelWare>();

            // Enable Swagger for both Dev and Production (ÚÔÇä ÊÌÑÈ ÈÑÇÍÊß)
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Smart Egypt API V1");
                c.RoutePrefix = "swagger"; // ÈíÎáí ÇáÜ swagger åæ ÇáÕÝÍÉ ÇáÑÆíÓíÉ áæ ÚæÒÊ
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
                Path.Combine(builder.Environment.WebRootPath, "csv-data")),
                RequestPath = "/csv-data"
            });
            app.UseRouting();

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            // Redirect Root to Swagger (Íá ãÔßáÉ ÇáÕÝÍÉ ÇáÝÇÖíÉ)
            app.MapGet("/", context => {
                context.Response.Redirect("/swagger");
                return Task.CompletedTask;
            });

            await app.RunAsync();
        }
    }
}
