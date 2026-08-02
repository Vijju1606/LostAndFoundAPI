using Microsoft.AspNetCore.Mvc;
using LostAndFoundAPI.Services.Interfaces;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using LostAndFoundAPI.DTOs;
using LostAndFoundAPI.Repositories.Interfaces;
namespace LostAndFoundAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LostItemsController : ControllerBase
    {
        private readonly ILostItemService _service;
        private readonly IFileService _fileService;

        public LostItemsController(ILostItemService service, IFileService fileService)
        {
            _service = service;
            _fileService = fileService;
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateLostItem(CreateLostItemDto dto)
        {
            
          var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            
            var result =await _service.CreateLostItem(dto, userId);
            return Ok(result);
        } 


        [Authorize]
        [HttpGet("MyItems")]
        public IActionResult GetMyLostItems()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var lostItems = _service.GetMyLostItems(userId);
            return Ok(lostItems);
        }

        [Authorize]
        [HttpPut("{id}")]
        public  async Task<IActionResult> UpdateLostItem(int id,[FromForm] CreateLostItemDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _service.UpdateLostItem(id, dto, userId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteLostItem(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = _service.DeleteLostItem(id, userId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLostItemsAsync()
        {
            var result = await _service.GetAllLostItemsAsync();
            return Ok(result);
        }
       
       
       [HttpGet("{id}")]
        public async Task<IActionResult>GetbyId(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (!result.Success)
            {
                BadRequest(result);
            }
            return Ok(result);
        }
    }
}