using LostAndFoundAPI.Common;

namespace LostAndFoundAPI.Services.Interfaces{
public interface IAdminService
{
    Task<ApiResponse>GetDashboardAsync();
    Task<ApiResponse>GetAllUsersAsync();
    Task<ApiResponse>UpdateUserRoleAsync(int userId,int currentAdminId,UpdateUsersRoleDto dto);
    Task<ApiResponse>SearchUsersAsync(string keyword);
    Task<ApiResponse> GetAllLostItemsAsync();
    Task<ApiResponse>GetAllFoundItemsAsync();
    Task<ApiResponse>DeleteLostItemAsync(int id);
    Task<ApiResponse>DeleteFoundItemAsync(int id);
    Task<ApiResponse>GetAllContactRequestsAsync();
}
}