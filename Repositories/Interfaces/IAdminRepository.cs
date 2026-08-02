using LostAndFoundAPI.Common;
using LostAndFoundAPI.DTOs;
using LostAndFoundAPI.Models;

namespace LostAndFoundAPI.Repositories.Interfaces{
public interface IAdminRepository
{
    Task<AdminDashboardDto>GetDashboardAsync();
    Task<List<UserManagementDto>>GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(int userId);
    Task DeleteUserAsync(User user);
    Task<List<UserManagementDto>>SearchUsersAsync(string keyword);
    Task <List<AdminLostItemDto>>GetAllLostItemAsync();
    Task<List<AdminFoundItemDto>>GetAllFoundItemsAsync();
    Task <LostItem?>GetLostItemByIdAsync(int id);
    

    Task DeleteLostItemAsync(LostItem lostItem);
    Task<bool>DeleteFoundItemAsync(int id);
    Task SaveChangesAsync();

    Task<List<ContactRequestViewDto>>GetAllContactRequestsAsync();
}
}
