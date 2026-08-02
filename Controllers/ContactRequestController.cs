using LostAndFoundAPI.Services.Implementations;
using LostAndFoundAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace LostAndFoundAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactRequestController : ControllerBase
    {
        private readonly IContactRequestService _service;
        public ContactRequestController(IContactRequestService service)
        {
            _service=service;
        }


      [HttpPost("send")] 
      public async Task<IActionResult> SendContactRequest(SendContactRequestDto dto)
        {
           var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
           
           var result = await _service.SendContactRequestAsync(
            dto.LostItemId,
            dto.FoundItemId,
            dto.MatchScore,
            userId
            
           ) ;

            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingRequests()
        {
            var userId=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result= await _service.GetPendingRequestsAsync(userId);
            return Ok(result);
        }

        [Authorize]
        [HttpPut("approve")]
        public async Task<IActionResult> ApproveRequest(ApproveContactRequestDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _service.ApproveRequestAsync(dto,userId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [Authorize]
       [HttpPut("reject")]
       public async Task<IActionResult> RejectRequest(RejectRequestDto dto)
        {
            var userId= int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _service.RejectRequestAsync(dto ,userId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);

        }
     [Authorize]
        [HttpGet("request")]
        public async Task<IActionResult> MyRequests()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _service.GetMyRequestsAsync(userId);
            return Ok(result);
        }

    }
    
     
}