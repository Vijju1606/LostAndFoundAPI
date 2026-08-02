using System.Text.Json;
using LostAndFoundAPI.Common;
using LostAndFoundAPI.DTOs;
using LostAndFoundAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFoundAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordResetOtpController : ControllerBase
    {
        private readonly IPasswordResetOtpService _services;
        public PasswordResetOtpController(IPasswordResetOtpService service)
        {
            _services=service;
        }


        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] JsonElement payload)
        {
            var email = ExtractString(payload, "email") ?? ExtractString(payload, "Email");
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Email is required."
                });
            }

            var dto = new ForgetPasswordDto { Email = email };
            var result = await _services.ForgetPasswordAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("verifyotp")]
        public async Task<IActionResult> VerifyOtp([FromBody] JsonElement payload)
        {
            var email = ExtractString(payload, "email") ?? ExtractString(payload, "Email");
            var otp = ExtractString(payload, "otp") ?? ExtractString(payload, "OTP");

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(otp))
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Email and OTP are required."
                });
            }

            var dto = new VerifyOtpDto { Email = email, OTP = otp };
            var result = await _services.VerifyOtpAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] JsonElement payload)
        {
            var email = ExtractString(payload, "email") ?? ExtractString(payload, "Email");
            var newPassword = ExtractString(payload, "newPassword") ?? ExtractString(payload, "NewPassword");
            var confirmPassword = ExtractString(payload, "confirmPassword") ?? ExtractString(payload, "ConfirmPassword");

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Email, new password, and confirm password are required."
                });
            }

            var dto = new ResetPasswordDto { Email = email, NewPassword = newPassword, ConfirmPassword = confirmPassword };
            var result = await _services.ResetPasswordAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        private static string? ExtractString(JsonElement payload, string propertyName)
        {
            if (payload.ValueKind != JsonValueKind.Object)
            {
                return null;
            }

            if (payload.TryGetProperty(propertyName, out var property))
            {
                return property.ValueKind == JsonValueKind.String ? property.GetString() : property.ToString();
            }

            return null;
        }

    }
}