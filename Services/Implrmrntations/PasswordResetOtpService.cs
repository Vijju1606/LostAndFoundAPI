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
            var user = await _repository.GetUserByEmailAsync(dto.Email);
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
                UserId= user.UserId,
                OTP=otp,
                CreatedAt =now,
                ExpiryTime= now.AddMinutes(10)

            };

            await _repository.UpsertOtpAsync(passwordResetOtp);

            try
            {
                await _emailService.SendEmailAsync(
                    user.Email,
                    "Reset Password OTP",$"Your OTP is {otp}. It is valid for 10 minutes."
                    );
                    return new ApiResponse{
                        Success = true,
                        Message="OTP sent Successfully"

        
                };


            }

            catch( Exception  )
            {
                await _repository.DeleteOtpAsync(passwordResetOtp);
                return new ApiResponse
                {
                    Success=false,
                    Message="Unable to send OTP,Please try again."
                };
                
            }
        }

        public async Task<ApiResponse> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _repository.GetUserByEmailAsync(dto.Email);
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
            var user = await _repository.GetUserByEmailAsync(dto.Email);
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
            if (passwordResetOtp.OTP != dto.OTP)
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