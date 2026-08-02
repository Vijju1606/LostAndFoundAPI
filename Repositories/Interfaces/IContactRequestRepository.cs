using LostAndFoundAPI.DTOs;
using LostAndFoundAPI.Models;
namespace LostAndFoundAPI.Repositories.Interfaces
{
    public interface IContactRequestRepository
    {
        Task<ContactRequest?>GetByIdAsync(int contactRequestId);
        Task<ContactRequest?>GetExistingRequestAsync(int? lostItemId,int foundItemid);
        Task<ContactRequest?>GetExistingDirectRequestAsync(int requestedByUserId,int foundItemId);
        Task<IEnumerable<PendingContactRequestDto>>GetPendingRequestAsync(int userId);
         Task<IEnumerable<ContactRequestViewDto>>GetMyRequestsAsync(int userId);
         Task AddAsync(ContactRequest request);
         Task SaveChangesAsync();

    }
}
