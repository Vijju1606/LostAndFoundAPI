using LostAndFoundAPI.DTOs;
using LostAndFoundAPI.Common;   
using LostAndFoundAPI.Models;


namespace LostAndFoundAPI.Repositories.Interfaces
{
    public interface IFoundItemRepository
    {
        Task<ApiResponse> CreateFoundItem(CreateFoundItemDto dto, int userId);
        ApiResponse GetMyFoundItems(int userId);
        ApiResponse UpdateFoundItem(int id, CreateFoundItemDto dto, int userId);
        ApiResponse DeleteFoundItem(int id, int userId);
        
        List<FoundItem> GetAllFoundItems();
        Task<FoundItem?> GetByIdAsync(int id);
        Task<bool>MarkAsReturnedAsync(int foundItemId,int userId);
        
}}