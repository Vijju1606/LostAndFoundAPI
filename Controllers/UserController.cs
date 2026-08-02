using Microsoft.AspNetCore.Mvc;
using LostAndFoundAPI.Data;

namespace LostAndFoundAPI.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UserController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetUser()
        {
            return Ok(_context.Users.ToList());
        }
    }
}