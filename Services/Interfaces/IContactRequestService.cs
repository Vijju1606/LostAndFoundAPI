using LostAndFoundAPI.Common;

namespace LostAndFoundAPI.Services.Interfaces
{
    public interface IContactRequestService
    {
        Task<ApiResponse>SendContactRequestAsync(int? lostItemId, int foundItemId,int? matchScore,int userId);
        Task<ApiResponse>GetMyRequestsAsync(int userId);
        Task<ApiResponse>GetPendingRequestsAsync( int userId);
        Task<ApiResponse>ApproveRequestAsync(ApproveContactRequestDto dto , int userId);
        Task<ApiResponse>RejectRequestAsync(RejectRequestDto dto, int userId);

    }
}
