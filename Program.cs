using LostAndFoundAPI.Data;
using Microsoft.EntityFrameworkCore;
using LostAndFoundAPI.Repositories.Interfaces;
using LostAndFoundAPI.Repositories.Implementations;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LostAndFoundAPI.Services.Interfaces;
using LostAndFoundAPI.Services.Implementations;
using LostAndFoundAPI.Common;




var builder = WebApplication.CreateBuilder(args);

var jwtKey = builder.Configuration["jwt:key"];
if (string.IsNullOrWhiteSpace(jwtKey))
{
    jwtKey = "dev-secret-key-change-in-production-123";
}

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ILostItemRepository, LostItemRepository>();
builder.Services.AddScoped<IFoundItemRepository, FoundItemRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMatchService,MatchService>();
builder.Services.AddScoped<ILostItemService, LostItemService>();
builder.Services.AddScoped<IFoundItemService,FoundItemService>();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IFileService, FileService>();
 builder.Services.AddScoped<IEmailService,EmailService>();
 builder.Services.AddScoped<IPasswordResetOtpRepository,PasswordResetOtpRepository>();
 builder.Services.AddScoped<IPasswordResetOtpService,PasswordResetOtpService>();
 builder.Services.AddScoped<IContactRequestService,ContactRequestService>();
 builder.Services.AddScoped<IContactRequestRepository,ContactRequestRepository>();
 builder.Services.AddScoped<IAdminRepository,AdminRepository>();
 builder.Services.AddScoped<IAdminService,AdminService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["jwt:Issuer"],
            ValidAudience = builder.Configuration["jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

    builder.Services.Configure<EmailSettings>(
        builder.Configuration.GetSection("EmailSettings")
    );





builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .SetIsOriginAllowed(_=> true)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
   

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseMiddleware<LostAndFoundAPI.Middleware.ExceptionMiddleware>();
app.UseStaticFiles();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();