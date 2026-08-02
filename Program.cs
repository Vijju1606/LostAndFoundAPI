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
    throw new InvalidOperationException("JWT key is not configured. Set Jwt__Key in the deployment environment.");
}

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Database connection string is not configured. Set ConnectionStrings__DefaultConnection in the deployment environment.");
}

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

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





var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
var isDevelopment = builder.Environment.IsDevelopment();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .SetIsOriginAllowed(origin =>
            {
                if (string.IsNullOrWhiteSpace(origin))
                {
                    return false;
                }

                if (allowedOrigins.Contains(origin, StringComparer.OrdinalIgnoreCase))
                {
                    return true;
                }

                if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri))
                {
                    return false;
                }

                return isDevelopment && (uri.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase)
                    || uri.Host.Equals("127.0.0.1", StringComparison.OrdinalIgnoreCase)
                    || uri.Host.EndsWith(".localhost", StringComparison.OrdinalIgnoreCase));
            })
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
