using LostAndFoundAPI.Common;
using LostAndFoundAPI.DTOs;
namespace LostAndFoundAPI.Services.Interfaces
{
    public interface IAuthService
    {
        ApiResponse Register(RegisterDto dto);
        ApiResponse Login(LoginDto dto);
    }
}