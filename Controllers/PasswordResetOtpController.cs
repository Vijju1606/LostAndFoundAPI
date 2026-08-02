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
        public async Task<IActionResult> ForgotPassword([FromBody]ForgetPasswordDto dto)
        {
            var result= await _services.ForgetPasswordAsync(dto);
            return Ok(result);
        }

        [HttpPost("verifyotp")]
        public async Task<IActionResult>VerifyOtp( [FromBody]VerifyOtpDto dto)
        {
            var result = await _services.VerifyOtpAsync(dto);
            return Ok(result);
        }
        [HttpPost("resetpassword")]
        public async Task<IActionResult>ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var result = await _services.ResetPasswordAsync(dto);
            return Ok(result);
        }

    }
}