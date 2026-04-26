using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Persistence.Data.Repositories.Repo.DashboardRepository;
using Service.ServiceImplemmentation.DashboardService;
using DomainLayer.Contracts.Repo.DashboardRepo;
using ServiceAbstraction.Services.DashboardServices;
using Persistence.Data.configurations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<SmartEgyptDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// DI
builder.Services.AddScoped<IDashboardAuthRepo, DashboardAuthRepo>();
builder.Services.AddScoped<IDashboardAuthService, DashboardAuthService>();
builder.Services.AddScoped<IDashboardOwnerRepo, DashboardOwnerRepo>();
builder.Services.AddScoped<IDashboardOwnerService, DashboardOwnerService>();
builder.Services.AddScoped<IDashboardAdminRepo, DashboardAdminRepo>();
builder.Services.AddScoped<IDashboardAdminService, DashboardAdminService>();
// AI Services :
builder.Services.AddSingleton<CsvService>();

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

// JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["JwtSettings:SecretKey"]!))
        };
    });
//Auth Email Service :
builder.Services.AddScoped<DashEmailService>();

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.UseCors("AllowAll");
app.UseStaticFiles(); 
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
