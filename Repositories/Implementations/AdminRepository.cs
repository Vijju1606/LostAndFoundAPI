using LostAndFoundAPI.Repositories.Interfaces;
using LostAndFoundAPI.Data;
using Microsoft.EntityFrameworkCore;
using LostAndFoundAPI.Common;
using LostAndFoundAPI.Models;
using System.ComponentModel.Design;
using LostAndFoundAPI.DTOs;

namespace LostAndFoundAPI.Repositories.Implementations
{
    public class AdminRepository: IAdminRepository
    {
        private readonly AppDbContext _context;
        public AdminRepository(AppDbContext context)
        {
            _context= context;
        }
    

    public async Task<AdminDashboardDto> GetDashboardAsync()
        {
            var dashboard = new AdminDashboardDto
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalLostItems=await _context.LostItems.CountAsync(),
                TotalFoundItems=await _context.FoundItems.CountAsync(),
                ReturnedItems=await _context.FoundItems.CountAsync(x=> x.IsReturned),
                PendingRequests=await _context.ContactRequests.CountAsync(x=>x.Status=="Pending")
        
            };
            return dashboard;
        }

        public async Task<List<UserManagementDto>> GetAllUsersAsync()
        {
            var users = await _context.Users.Select(user => new UserManagementDto
            {
                UserId = user.UserId,
                Name=user.Name,
                Email=user.Email,
                Role=user.Role
            }).ToListAsync();

            return users;
        }

        public async Task<User?>GetUserByIdAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(x=> x.UserId == userId);
        }

        public async Task<List<UserManagementDto>> SearchUsersAsync( string keyword)
        {
            var searchKeyword =keyword.ToLower().Replace(" ","");
            return await _context.Users.Where(user =>user.Name.ToLower().Replace(" ","").Contains(searchKeyword)|| user.Email.ToLower().Replace(" ","").Contains(searchKeyword)).Select(user=> new UserManagementDto
            {
                UserId=user.UserId,
                Name=user.Name,
                Email=user.Email,
                Role=user.Role
            }).ToListAsync();
        }

        public async Task <List<AdminLostItemDto>>GetAllLostItemAsync()
        {
            var lostItems = from lostItem in _context.LostItems
            join user in _context.Users 
            on lostItem.UserId equals user.UserId
            select new AdminLostItemDto
            {
                Id=lostItem.Id,
                ItemName=lostItem.ItemName,
                Description=lostItem.Description,
                Location=lostItem.Location,
                DateLost= lostItem.DateLost,
                ImageUrl=lostItem.ImageUrl,
                OwnerName=user.Name,
                OwnerEmail= user.Email
            };
            return await lostItems.ToListAsync();
        }



        public async Task<List<AdminFoundItemDto>>GetAllFoundItemsAsync()
        {
            var foundItems =
                from foundItem in _context.FoundItems
                join user in _context.Users
                on foundItem.UserId equals user.UserId
                select new AdminFoundItemDto
                {
                    Id = foundItem.Id,
                    ItemName=foundItem.ItemName,
                    Description=foundItem.Description,
                    Location=foundItem.Location,
                    DateFound=foundItem.DateFound,
                    ImageUrl=foundItem.ImageUrl,
                    IsReturned=foundItem.IsReturned,
                    OwnerName=user.Name,
                    OwnerEmail=user.Email
                };
                return await foundItems.ToListAsync();
            
        }



        public async Task<LostItem?>GetLostItemByIdAsync(int id)
        {
            return await _context.LostItems.FirstOrDefaultAsync(x=> x.Id==id);
        }
        public async Task DeleteLostItemAsync(LostItem lostItem)
        {
            _context.LostItems.Remove(lostItem);
            await _context.SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }


        public async Task<bool>DeleteFoundItemAsync(int id)
        {
            var foundItem = await _context.FoundItems.FirstOrDefaultAsync(x=> x.Id == id);
            if (foundItem == null)
            {
                return false;
            }
            _context.FoundItems.Remove(foundItem);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<List<ContactRequestViewDto>> GetAllContactRequestsAsync()
{
    var requests =
        from request in _context.ContactRequests

        join lostItem in _context.LostItems
            on request.LostItemId equals lostItem.Id

        join foundItem in _context.FoundItems
            on request.FoundItemId equals foundItem.Id

        join requestedBy in _context.Users
            on request.RequestedByUserId equals requestedBy.UserId

        join requestedTo in _context.Users
            on request.RequestedToUserId equals requestedTo.UserId

        select new ContactRequestViewDto
        {
            ContactRequestId = request.ContactRequestId,
            LostItemName = lostItem.ItemName,
            FoundItemName = foundItem.ItemName,
            RequestedBy = requestedBy.Name,
            RequestedTo = requestedTo.Name,
            Status = request.Status,
            sharedPhoneNumber = request.SharedPhoneNumber,
            RequestedAt = request.RequestedAt,
            RespondedAt = request.RespondedAt
        };

    return await requests.ToListAsync();
}

}
}