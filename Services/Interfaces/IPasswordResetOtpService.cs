using LostAndFoundAPI.Common;
using LostAndFoundAPI.DTOs;

namespace LostAndFoundAPI.Services.Interfaces
{
    public interface IPasswordResetOtpService
    {
        Task<ApiResponse> ForgetPasswordAsync(ForgetPasswordDto dto);
        Task<ApiResponse> VerifyOtpAsync(VerifyOtpDto otp);
        Task<ApiResponse>ResetPasswordAsync(ResetPasswordDto dto);
        
    }
}