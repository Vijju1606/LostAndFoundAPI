using System.Security.Claims;
using LostAndFoundAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace LostAndFoundAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }
        [Authorize]
        [HttpGet("Lost/{lostItemId}")]
        public IActionResult GetMatches(int lostItemId)

        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var matches = _matchService.GetMatches(lostItemId,userId);
            return Ok(matches);
        }
    }
}