using Microsoft.AspNetCore.Mvc;
using LostAndFoundAPI.DTOs;
using LostAndFoundAPI.Repositories.Interfaces;
using LostAndFoundAPI.Common;
using LostAndFoundAPI.Services.Interfaces;

namespace LostAndFoundAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
           ApiResponse result = _authService.Register(dto);
           if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            ApiResponse result = _authService.Login(dto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}