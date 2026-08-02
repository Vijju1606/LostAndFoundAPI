using LostAndFoundAPI.DTOs;
using LostAndFoundAPI.Common;
using LostAndFoundAPI.Models;


namespace LostAndFoundAPI.Repositories.Interfaces
{
    public interface ILostItemRepository
    {
        Task<ApiResponse> CreateLostItem(CreateLostItemDto dto, int userId);
        ApiResponse GetMyLostItems(int userId);
        Task<ApiResponse> UpdateLostItem(int id, CreateLostItemDto dto, int userId);
        ApiResponse DeleteLostItem(int id, int userId);
         LostItem? GetLostItemById(int id , int userId);
         Task<LostItem?>GetByIdAsync(int id);
          Task <List<LostItem>> GetAllLostItemsAsync();
         
    }
}