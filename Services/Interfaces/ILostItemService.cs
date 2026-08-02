using LostAndFoundAPI.DTOs;
using LostAndFoundAPI.Common;
using LostAndFoundAPI.Models;


namespace LostAndFoundAPI.Services.Interfaces
{
    public interface ILostItemService
    {
        Task<ApiResponse> CreateLostItem(CreateLostItemDto dto, int userId);
        ApiResponse GetMyLostItems(int userId);
        Task<ApiResponse> UpdateLostItem(int id, CreateLostItemDto dto, int userId);
        ApiResponse DeleteLostItem(int id, int userId);
        LostItem? GetLostItemById(int id , int userId);
        Task<ApiResponse>GetAllLostItemsAsync();
        Task<ApiResponse>GetByIdAsync(int id);
      
    }
}