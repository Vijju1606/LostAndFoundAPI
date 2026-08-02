using System.Data;
using LostAndFoundAPI.Common;
using LostAndFoundAPI.Data;
using LostAndFoundAPI.Models;
using LostAndFoundAPI.Repositories.Interfaces;
using LostAndFoundAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace LostAndFoundAPI.Services.Implementations
{
    public class ContactRequestService : IContactRequestService
    { 
        private readonly IContactRequestRepository _contactRequestRepository;
        private readonly ILostItemRepository _lostItemRepository;
        private readonly IFoundItemRepository _foundItemRepository;
        private readonly IEmailService _emailService;
        private readonly AppDbContext _context;

        public ContactRequestService(
            IContactRequestRepository contactRequestRepository,
            ILostItemRepository lostItemRepository,
            IFoundItemRepository foundItemRepository,
            IEmailService emailService,
            AppDbContext context)
        {
            _contactRequestRepository = contactRequestRepository;
            _foundItemRepository = foundItemRepository;
            _lostItemRepository = lostItemRepository;
            _emailService = emailService;
            _context = context;
        }
        




















        public async Task<ApiResponse> ApproveRequestAsync(ApproveContactRequestDto dto, int userId)
        {
            var request = await _contactRequestRepository.GetByIdAsync(dto.ContactRequestId);
            if (request == null)
            {
                return new ApiResponse
                {
                    Success=false,
                    Message="Contact request not found."
                };
            }
            if(request.RequestedToUserId != userId)
            {
                return new ApiResponse
                {
                    Success=false,
                    Message="You are not authorized to approve this request."
                };
            }
            if (!request.Status.Equals("pending", StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Success=false,
                    Message="This request has already been processed."
                };
            }
            request.SharedPhoneNumber=dto.SharedPhoneNumber;
            request.Status="Approved";
            request.RespondedAt=DateTime.UtcNow;
            await _contactRequestRepository.SaveChangesAsync();
            await SendStatusUpdateNotificationAsync(request, "approved");

            return new ApiResponse
            {
                Success=true,
                Message="Request Approved successfully."
            };
        }

        public async Task<ApiResponse> GetMyRequestsAsync(int userId)
        {
            var requests = await _contactRequestRepository.GetMyRequestsAsync(userId);
            return new ApiResponse
            {
                Success=true,
                Message="your requests fetched succesfully. ",
                Data=requests
            };
        }

        public async Task<ApiResponse> GetPendingRequestsAsync(int userId)
        {
            var requests = await _contactRequestRepository.GetPendingRequestAsync(userId);
            return new ApiResponse
            {
                Success=true,
                Message="Pending contact requests fetched succesfully.",
                Data = requests
            };
        }

        public async Task<ApiResponse> RejectRequestAsync(RejectRequestDto dto, int userId)
        {
            
            var request = await _contactRequestRepository.GetByIdAsync(dto.ContactRequestId);
            if(request == null)
            {
                return new ApiResponse
                {
                    Success=false,
                    Message="Request not Found"
                };
            }

             if(request.RequestedToUserId != userId)
            {
                return new ApiResponse
                {
                    Success=false,
                    Message="You are not authorized to reject this request."
                };
            }
         
            if (!request.Status.Equals("Pending", StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Success=false,
                    Message="This request has already been processed."
                };
            }
            request.Status="Rejected";
            request.RespondedAt=DateTime.UtcNow;
            await _contactRequestRepository.SaveChangesAsync();
            await SendStatusUpdateNotificationAsync(request, "rejected");

            return new ApiResponse
            {
                Success=true,
                Message="Request Rejected succefully."
            };



        }

        public async Task<ApiResponse> SendContactRequestAsync(int? lostItemId, int foundItemId,int? matchScore, int userId)
        {
            var foundItem =await _foundItemRepository.GetByIdAsync(foundItemId);
            if (foundItem== null)
            {
                return new ApiResponse
                {
                    Success=false,
                    Message="Found item not found."
                };
            }
            if (foundItem.IsReturned)
            {
                return new ApiResponse
                {
                    Success=false,
                    Message="This item has already been returned."
                };
            }
            if (foundItem.UserId == userId)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "You cannot send a contact request for your own item."
                };
            }

            LostItem? lostItem = null;
            if (lostItemId.HasValue)
            {
                lostItem = await _lostItemRepository.GetByIdAsync(lostItemId.Value);
                if (lostItem == null)
                {
                    return new ApiResponse { Success=false, Message="Lost item not found." };
                }
                if (lostItem.UserId != userId)
                {
                    return new ApiResponse { Success=false, Message="You are not authorized to use this lost item." };
                }
            }
            if (lostItem != null && lostItem.UserId == foundItem.UserId)
            {
                return new ApiResponse
                {
                    Success=false,
                    Message="you cannot send a contact request to your own item."
                };
            }

            var existingRequest = lostItemId.HasValue
                ? await _contactRequestRepository.GetExistingRequestAsync(lostItemId, foundItemId)
                : await _contactRequestRepository.GetExistingDirectRequestAsync(userId, foundItemId);
            if(existingRequest != null)
            {
             return new ApiResponse
             {
                 Success=false,
                 Message="You have already sent a contact request for this match."
             }   ;
            }

            var request = new ContactRequest
            {
                LostItemId = lostItemId,
                FoundItemId=foundItemId,
                MatchScore=matchScore,
                RequestedByUserId=userId,
                RequestedToUserId=foundItem.UserId
            };
            await _contactRequestRepository.AddAsync(request);
            await _contactRequestRepository.SaveChangesAsync();
            await SendNewRequestNotificationAsync(request, foundItem, lostItem, userId);
            return new ApiResponse
            {
                Success=true,
                Message="Contact request sent successfully."
            };
            
        }

        private async Task SendNewRequestNotificationAsync(ContactRequest request, FoundItem foundItem, LostItem? lostItem, int requesterId)
        {
            var owner = await _context.Users.FirstOrDefaultAsync(x => x.UserId == foundItem.UserId);
            var requester = await _context.Users.FirstOrDefaultAsync(x => x.UserId == requesterId);

            if (owner == null || string.IsNullOrWhiteSpace(owner.Email))
            {
                return;
            }

            var subject = "New contact request received";
            var itemName = !string.IsNullOrWhiteSpace(foundItem.ItemName) ? foundItem.ItemName : "your found item";
            var lostItemName = lostItem != null && !string.IsNullOrWhiteSpace(lostItem.ItemName)
                ? lostItem.ItemName
                : "a reported lost item";
            var requesterName = requester?.Name ?? "Someone";
            var body = $"Hello {owner.Name},\n\n{requesterName} has sent you a contact request about {itemName}.\nThey are trying to connect regarding {lostItemName}.\n\nPlease review the request in the app.";

            await SendEmailSafelyAsync(owner.Email, subject, body);
        }

        private async Task SendStatusUpdateNotificationAsync(ContactRequest request, string status)
        {
            var requester = await _context.Users.FirstOrDefaultAsync(x => x.UserId == request.RequestedByUserId);
            if (requester == null || string.IsNullOrWhiteSpace(requester.Email))
            {
                return;
            }

            var subject = status.Equals("approved", StringComparison.OrdinalIgnoreCase)
                ? "Your contact request was approved"
                : "Your contact request was rejected";
            var body = status.Equals("approved", StringComparison.OrdinalIgnoreCase)
                ? $"Hello {requester.Name},\n\nYour contact request has been approved. The owner of the found item may contact you soon."
                : $"Hello {requester.Name},\n\nYour contact request has been rejected. You can try again with another match if needed.";

            await SendEmailSafelyAsync(requester.Email, subject, body);
        }

        private async Task SendEmailSafelyAsync(string to, string subject, string body)
        {
            try
            {
                await _emailService.SendEmailAsync(to, subject, body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Contact request notification failed: {ex.Message}");
            }
        }
    }

}
