using System.Security.Cryptography;
using LostAndFoundAPI.Common;
using LostAndFoundAPI.DTOs;
using LostAndFoundAPI.Models;
using LostAndFoundAPI.Repositories.Interfaces;
using LostAndFoundAPI.Services.Interfaces;

namespace LostAndFoundAPI.Services.Implementations
{
    public class PasswordResetOtpService : IPasswordResetOtpService
    {
        private readonly IPasswordResetOtpRepository _repository;
        private readonly IEmailService _emailService;


        public PasswordResetOtpService(IPasswordResetOtpRepository repository, IEmailService emailService)
        {
            _repository=repository;
            _emailService=emailService;
        }


        private string GenerateOtp()
        {
            return RandomNumberGenerator.GetInt32(100000,1000000).ToString();
        }









        public async Task<ApiResponse> ForgetPasswordAsync(ForgetPasswordDto dto)
        {
            var normalizedEmail = dto.Email?.Trim() ?? string.Empty;
            var user = await _repository.GetUserByEmailAsync(normalizedEmail);
            if(user == null)
            {
                return new ApiResponse
                {
                    Success=false,
                    Message="User not found"
                };
            }

            var now = DateTime.UtcNow;
            var otp = GenerateOtp();
            var passwordResetOtp = new PasswordResetOtp
            {
                UserId = user.UserId,
                OTP = otp,
                CreatedAt = now,
                ExpiryTime = now.AddMinutes(10)
            };

            try
            {
                await _emailService.SendEmailAsync(
                    user.Email,
                    "Reset Password OTP",
                    $"Your OTP is {otp}. It is valid for 10 minutes."
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OTP delivery failed: {ex.Message}");
                return new ApiResponse
                {
                    Success = false,
                    Message = "We could not send the OTP email. Please try again shortly."
                };
            }

            // Store an OTP only after its email was accepted by the SMTP server.
            await _repository.UpsertOtpAsync(passwordResetOtp);

            return new ApiResponse
            {
                Success = true,
                Message = "OTP sent to your email address."
            };
        }

        public async Task<ApiResponse> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var normalizedEmail = dto.Email?.Trim() ?? string.Empty;
            var user = await _repository.GetUserByEmailAsync(normalizedEmail);
            if(user == null)
            {
                return new ApiResponse
                {
                    Success=false,
                    Message="User not found."
                };
            }

            var passwordResetOtp = await _repository.GetOtpByUserIdAsync(user.UserId);
            if(passwordResetOtp == null)
            {
             return new ApiResponse
             {
                 Success=false,
                 Message="No OTP found.Please request a new OTP."
             };
            }

            if (DateTime.UtcNow > passwordResetOtp.ExpiryTime)
            {
                await _repository.DeleteOtpAsync(passwordResetOtp);
                return new ApiResponse
                {
                    Success=false,
                    Message="OTP has Expired.Please request new Otp."
                };
            }
            if (!passwordResetOtp.IsVerified)
            {
                return new ApiResponse
                {
                    Success=false,
                    Message="Please Verify your OTP first."
                };
            }

            if(dto.NewPassword != dto.ConfirmPassword)
            {
                return new ApiResponse
                {
                    Success=false,
                    Message="Password do not match."
                };
            }

            user.Password=BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _repository.SaveChangesAsync();
            await _repository.DeleteOtpAsync(passwordResetOtp);

            return new ApiResponse
            {
                Success=true,
                Message="Password reset Successfully."
            };
        }

        public async Task<ApiResponse> VerifyOtpAsync(VerifyOtpDto dto)
        {
            var normalizedEmail = dto.Email?.Trim() ?? string.Empty;
            var user = await _repository.GetUserByEmailAsync(normalizedEmail);
            if (user == null)
            {
                return new ApiResponse
                {
                    Success= false,
                    Message="User not found"
                };
            }
            var passwordResetOtp = await _repository.GetOtpByUserIdAsync(user.UserId);
            if (passwordResetOtp == null)
            {
                return new ApiResponse
                {
                    Success=false,
                    Message="No OTP found.Please request a new OTP."
                };
            }

            if(DateTime.UtcNow > passwordResetOtp.ExpiryTime)
            {
                await _repository.DeleteOtpAsync(passwordResetOtp);
                return new ApiResponse
                {
                    Success=false,
                    Message="Otp Expired.Please request new OTP."
                };
            }
            if (!string.Equals(passwordResetOtp.OTP, dto.OTP?.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Success=false,
                    Message="Invalid Otp"
                };
            }



            passwordResetOtp.IsVerified=true;
            await _repository.SaveChangesAsync();



            return new ApiResponse
            {
                Success=true,
                Message="OTP verified successfully."
            };
            

            
        }
    }
}
