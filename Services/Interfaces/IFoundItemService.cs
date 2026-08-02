using LostAndFoundAPI.DTOs;
using LostAndFoundAPI.Common;   
using LostAndFoundAPI.Models;


namespace LostAndFoundAPI.Services.Interfaces
{
    public interface IFoundItemService
    {
        Task<ApiResponse> CreateFoundItem(CreateFoundItemDto dto, int userId);
        ApiResponse GetMyFoundItems(int userId);
        Task<ApiResponse> UpdateFoundItem(int id, CreateFoundItemDto dto, int userId);
        ApiResponse DeleteFoundItem(int id, int userId);
        
        List<FoundItem> GetAllFoundItems();
        Task<ApiResponse>MarkAsReturnedAsync(int foundItemId,int userId);
        Task<ApiResponse>GetByIdAsync(int id);
        
}}
