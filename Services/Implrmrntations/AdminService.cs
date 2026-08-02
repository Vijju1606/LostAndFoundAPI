using LostAndFoundAPI.Services.Interfaces;
using LostAndFoundAPI.Repositories.Interfaces;
using LostAndFoundAPI.Common;
using System.Reflection.Metadata.Ecma335;
namespace LostAndFoundAPI.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _repository;
        public AdminService(IAdminRepository repository)
        {
            _repository=repository;
        }

       

        public async Task<ApiResponse> GetDashboardAsync()
        {
            var dashboard= await _repository.GetDashboardAsync();
            return new ApiResponse
            {
                Success=true,
                Message="Dashboard retrieved successfully.",
                Data=dashboard
            };


        }
        public async Task<ApiResponse> GetAllUsersAsync()
        {
            var users = await _repository.GetAllUsersAsync();
            return new ApiResponse
            {
                Success=true,
                Message="Users retrieved successfully.",
                Data = users
            };
        }

        public async Task<ApiResponse> DeleteUserAsync(int userId, int currentAdminId)
        {
            if (userId == currentAdminId)
            {
                return new ApiResponse { Success = false, Message = "You cannot remove your own account." };
            }

            var user = await _repository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return new ApiResponse { Success = false, Message = "User not found." };
            }

            await _repository.DeleteUserAsync(user);
            return new ApiResponse { Success = true, Message = "User removed successfully." };
        }

        public async Task<ApiResponse>UpdateUserRoleAsync(int userId,int currentAdminId,UpdateUsersRoleDto dto)
        {
            var user = await _repository.GetUserByIdAsync(userId);
            if(user== null)
            {
                return new ApiResponse
                {
                    Success=false,
                    Message="User not found."
                };
            }
            if(userId == currentAdminId)
            {
                return new ApiResponse
                {
                    Success=false,
                    Message="You cannot change your own role."
                };
            }
            if(dto.Role !="Admin"&& dto.Role != "User")
            {
                return new ApiResponse
                {
                    Success= false,
                    Message="Invalid role."
                };
            }
            user.Role=dto.Role;
            await _repository.SaveChangesAsync();
            return new ApiResponse
            {
                Success= true,
                Message="User role updated successfully."
            };
        }


        public async Task<ApiResponse>SearchUsersAsync(string keyword)
        {
            keyword=keyword?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return new ApiResponse
                {
                    Success=false,
                    Message="Search keyword is required."
                };
            }

            var users =await _repository.SearchUsersAsync(keyword);
            return new ApiResponse
            {
                Success=true,
                Message="Users retrieved successsfully.",
                Data=users
            };
        }

        public async Task<ApiResponse> GetAllLostItemsAsync()
        {
            var lostItems= await _repository.GetAllLostItemAsync();
            return new ApiResponse
            {
                Success=true,
                Message="lost items retrieved successfully.",
                Data=lostItems
            };
        }

        public async Task<ApiResponse>DeleteLostItemAsync(int id)
        {
            var lostItem= await _repository.GetLostItemByIdAsync(id);
            if(lostItem == null)
            {
                return new ApiResponse
                {
                    Success=false,
                    Message="Lost item not found."
                };
            }
            await _repository.DeleteLostItemAsync(lostItem);
            return new ApiResponse
            {
                Success=true,
                Message="Lost item deleted Successfully."
            };
        }

        public async Task<ApiResponse> GetAllFoundItemsAsync()
        {
            var foundItems=await _repository.GetAllFoundItemsAsync();
            return new ApiResponse
            {
                Success=true,
                Message="Found items retrieved successfully.",
                Data=foundItems
            };
        }

        public async Task<ApiResponse>DeleteFoundItemAsync(int id)
        {
            var deleted = await _repository.DeleteFoundItemAsync(id);
            if (!deleted)
            {
                return new ApiResponse
                {
                    Success=false,
                    Message= " Found itrm not found."
                };

            }
            return new ApiResponse
            {
                Success=true,
                Message="Found item deleted successfully."
            };
        }

        public async Task<ApiResponse> GetAllContactRequestsAsync()
        {
            var requests= await _repository.GetAllContactRequestsAsync();
            return new ApiResponse
            {
                Success=true,
                Message="Contact requests retrieved successfully.",
                Data=requests
            };
        }

    }
}
