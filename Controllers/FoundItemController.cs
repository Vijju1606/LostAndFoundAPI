using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using LostAndFoundAPI.DTOs;
using LostAndFoundAPI.Repositories.Interfaces;
using LostAndFoundAPI.Services.Interfaces;


namespace LostAndFoundAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoundItemController : ControllerBase
    {
        private readonly IFoundItemService _service;

        public FoundItemController(IFoundItemService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateFoundItem([FromForm] CreateFoundItemDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _service.CreateFoundItem(dto, userId);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("MyItems")]
        public IActionResult GetMyFoundItems()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = _service.GetMyFoundItems(userId);
            return Ok(result);
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateFoundItem(int id, CreateFoundItemDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = _service.UpdateFoundItem(id, dto, userId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public  IActionResult DeleteFoundItem(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = _service.DeleteFoundItem(id, userId);
            if (!result.Success)
            {
                return  BadRequest(result);
            }
            return  Ok(result);
        }

        [HttpGet]
        public IActionResult GetAllFoundItems()
        {
            var result = _service.GetAllFoundItems();
            return Ok(result);
        }

        [Authorize]
        [HttpPut("{id}/return")]
        public async Task<IActionResult>MarkAsReturned(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _service.MarkAsReturnedAsync(id,userId);
            if (!result.Success)
            {
               return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult>GetByIdAsync(int id)
        {
            var result=await _service.GetByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
