using System.Security.Claims;
using LostAndFoundAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _service;
    public AdminController(IAdminService service)
    {
        _service= service;
    }


    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        var dashboard = await _service.GetDashboardAsync();
        return Ok(dashboard);
    }
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsersAsync()
    {
       var result= await _service.GetAllUsersAsync();
        return Ok(result);

    }

    [HttpDelete("users/{userId}")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        var currentAdminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _service.DeleteUserAsync(userId, currentAdminId);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpPut("users/{userId}/role")]
    public async Task<IActionResult>UpdateUserRole(int userId,UpdateUsersRoleDto dto)
    {
        var currentAdminId=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _service.UpdateUserRoleAsync(userId,currentAdminId,dto);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpGet("users/search")]
    public async Task<IActionResult>SearchUsers([FromQuery]string keyword)
    {
        var result = await _service.SearchUsersAsync(keyword);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpGet("lost-items")]
    public async Task<IActionResult> GetAllLostItems()
    {
        var result =await _service.GetAllLostItemsAsync();
        return Ok(result);
    }


    [HttpDelete("lost-Item/{id}")]
    public async Task<IActionResult>DeleteLostItem(int id)
    {
        var result = await _service.DeleteLostItemAsync(id);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpGet("found-items")]
    public async Task<IActionResult> GetAllFoundItems()
    {
        var result = await _service.GetAllFoundItemsAsync();
        return Ok(result);
    }

    [HttpDelete("found-item/{id}")]
    public async Task<IActionResult>DeleteFoundItem(int id)
    {
        var result = await _service.DeleteFoundItemAsync(id);
        if (!result.Success)
        {
           return BadRequest(result);
        }
        return Ok(result);
    }


    [HttpGet("contact-requests")]
    public async Task<IActionResult> GetAllContactRequests()
    {
        var result = await _service.GetAllContactRequestsAsync();
        return Ok(result);
    }
}
