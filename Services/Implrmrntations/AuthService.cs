using LostAndFoundAPI.Common;
using LostAndFoundAPI.DTOs;
using LostAndFoundAPI.Repositories.Interfaces;
using LostAndFoundAPI.Services.Interfaces;
using LostAndFoundAPI.Data;
using LostAndFoundAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace LostAndFoundAPI.Services.Implementations
{
    public class AuthService : IAuthService
    {

        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var jwtKey = _configuration["jwt:key"];
            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                jwtKey = "dev-secret-key-change-in-production-123";
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["jwt:Issuer"],
                audience: _configuration["jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
       




       public ApiResponse Register(RegisterDto dto)
        {
          if (_context.Users.Any(x => x.Email == dto.Email))
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "User with this email already exists.",
                    
                };
            }
        User user = new User()
        {
            Name = dto.Name,
            Email = dto.Email,
            Password =BCrypt.Net.BCrypt.HashPassword( dto.Password),
            Role = "User"
        };
        _context.Users.Add(user);
        _context.SaveChanges();
        return new ApiResponse
        {
            Success = true,
            Message = "User registered successfully.",
            
        };
        }



         public ApiResponse Login(LoginDto dto)
        {
            User user = _context.Users.FirstOrDefault(x => x.Email == dto.Email);
            
            if (user == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message ="Invalid email or password."
                };
            }
            if (! BCrypt.Net.BCrypt.Verify(dto.Password,user.Password))
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }
            return new ApiResponse
            {
                Success = true,
                Message = "Login successful.",
                Token = GenerateJwtToken(user)
            };
            
        }

    }
}
