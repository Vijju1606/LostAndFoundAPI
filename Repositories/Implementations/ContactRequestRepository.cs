using System.Reflection.Metadata.Ecma335;
using LostAndFoundAPI.Data;
using LostAndFoundAPI.DTOs;
using LostAndFoundAPI.Models;
using LostAndFoundAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;



namespace LostAndFoundAPI.Repositories.Implementations
{
    public class ContactRequestRepository : IContactRequestRepository
    {
        private readonly AppDbContext _context;
        public ContactRequestRepository(AppDbContext context)
        {
            _context=context;
        }


















        public async Task AddAsync(ContactRequest request)
        {
            await _context.ContactRequests.AddAsync(request);
        }

        public async Task<ContactRequest?> GetByIdAsync(int contactRequestId)
        {
            return await _context.ContactRequests.FirstOrDefaultAsync(x=> x.ContactRequestId == contactRequestId);
        }

        public async Task<ContactRequest?> GetExistingRequestAsync(int? lostItemId, int foundItemid)
        {
            return await _context.ContactRequests.FirstOrDefaultAsync(x=> x.LostItemId== lostItemId &&
             x.FoundItemId== foundItemid  && (x.Status=="Pending" || x.Status=="Approved"));
        }

        public async Task<ContactRequest?> GetExistingDirectRequestAsync(int requestedByUserId, int foundItemId)
        {
            return await _context.ContactRequests.FirstOrDefaultAsync(x =>
                x.RequestedByUserId == requestedByUserId && x.FoundItemId == foundItemId &&
                x.LostItemId == null && (x.Status == "Pending" || x.Status == "Approved"));
        }

        public async Task<IEnumerable<ContactRequestViewDto>> GetMyRequestsAsync(int userId)
        {
          var myRequests =from contactRequest in _context.ContactRequests

                           where contactRequest.RequestedByUserId == userId

                         join lostItem in _context.LostItems
                          on contactRequest.LostItemId equals lostItem.Id

                           join foundItem in _context.FoundItems
                           on contactRequest.FoundItemId equals foundItem.Id

                          join requestedBy in _context.Users
                          on contactRequest.RequestedByUserId equals requestedBy.UserId

                        join requestedTo in _context.Users
                        on contactRequest.RequestedToUserId equals requestedTo.UserId

    select new ContactRequestViewDto
    {
        ContactRequestId = contactRequest.ContactRequestId,

        LostItemName = lostItem.ItemName,

        FoundItemName = foundItem.ItemName,

        RequestedBy = requestedBy.Name,

        RequestedTo = requestedTo.Name,

        Status = contactRequest.Status,

        sharedPhoneNumber = contactRequest.SharedPhoneNumber,

        RequestedAt = contactRequest.RequestedAt,

        RespondedAt = contactRequest.RespondedAt
    };

return await myRequests.ToListAsync();
        }

        public async Task<IEnumerable<PendingContactRequestDto>> GetPendingRequestAsync(int userId)
        {
            var pendingRequests=from ContactRequest in _context.ContactRequests 
            where ContactRequest.RequestedToUserId==userId &&
                  (ContactRequest.Status=="pending" || ContactRequest.Status=="Approved")
            join LostItem in _context.LostItems on ContactRequest.LostItemId equals LostItem.Id into lostItems
            from LostItem in lostItems.DefaultIfEmpty()
            join FoundItem in _context.FoundItems on ContactRequest.FoundItemId equals FoundItem.Id
            join user in _context.Users on ContactRequest.RequestedByUserId equals user.UserId
            select new PendingContactRequestDto
            {
                ContactRequestId = ContactRequest.ContactRequestId,
                FoundItemId = ContactRequest.FoundItemId,
                
                LostItemTitle=LostItem == null ? "No lost item reported" : LostItem.ItemName,
                LostItemDescription=LostItem == null ? "User just wants to contact you." : LostItem.Description,

                FoundItemTitle=FoundItem.ItemName,
                FoundItemDescription=FoundItem.Description,
                RequestedByName=user.Name,
                RequestedByEmail=user.Email,
                RequestedAt = ContactRequest.RequestedAt,
                Status= ContactRequest.Status,
                MatchScore=ContactRequest.MatchScore
            };
            return await pendingRequests.ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
