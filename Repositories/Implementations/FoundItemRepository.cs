using LostAndFoundAPI.Data;
using LostAndFoundAPI.DTOs;
using LostAndFoundAPI.Models;
using LostAndFoundAPI.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using LostAndFoundAPI.Repositories.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace LostAndFoundAPI.Repositories.Implementations
{
   public class FoundItemRepository : IFoundItemRepository
    {
        private readonly AppDbContext _Context;

        public FoundItemRepository(AppDbContext Context)
        {
            _Context = Context;
        }

     public  async Task<ApiResponse> CreateFoundItem(CreateFoundItemDto dto, int userId)
        {
            var foundItem = new FoundItem();
            foundItem.ItemName = dto.ItemName;
            foundItem.Description = dto.Description;
            foundItem.Location = dto.Location;
            foundItem.DateFound = dto.DateFound;
            foundItem.ImageUrl=dto.ImageUrl;
            foundItem.UserId = userId;

            _Context.FoundItems.Add(foundItem);
            _Context.SaveChanges();

            return new ApiResponse
            {
                Success = true,
                Message = "Found item created successfully."
            };
        }

        public ApiResponse GetMyFoundItems(int userId)
        {
            var foundItems = _Context.FoundItems.Where(x => x.UserId == userId && !x.IsReturned).ToList();
            return new ApiResponse
            {
                Success = true,
                Message = "Found items retrieved successfully.",
                Data = foundItems
            };
        }

        public ApiResponse UpdateFoundItem(int id, CreateFoundItemDto dto, int userId)
        {
            var foundItem =_Context.FoundItems.FirstOrDefault(x=> x.Id == id);
            if (foundItem == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Found item not found."
                };
            }
            if(foundItem.UserId != userId)
                {
                    return new ApiResponse
                    {
                        Success = false,
                        Message = "You are not authorized to update this found item."
                    };
                }
            
            foundItem.ItemName = dto.ItemName;
            foundItem.Description = dto.Description;
            foundItem.Location = dto.Location;
            foundItem.DateFound = dto.DateFound;
            _Context.SaveChanges();
            return new ApiResponse
            {
                Success = true,
                Message = "Found item updated successfully."
            };
        }

        public ApiResponse DeleteFoundItem(int id, int userId)
        {
            var foundItem = _Context.FoundItems.FirstOrDefault(x => x.Id == id);
            if (foundItem == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Found item not found."
                };
            }
            if (foundItem.UserId != userId)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "You are not authorized to delete this found item."
                };
            }
            _Context.FoundItems.Remove(foundItem);
            _Context.SaveChanges();
            return new ApiResponse
            {
                Success = true,
                Message = "Found item deleted successfully."
            };

        }


        public async Task<FoundItem>GetByIdAsync(int id)
        {
            return await _Context.FoundItems.FirstOrDefaultAsync(x=> x.Id == id  && !x.IsReturned) ;
        }


        // for matching results
        
        public List<FoundItem> GetAllFoundItems()
        {
            return _Context.FoundItems.Where(x=> !x.IsReturned).ToList();
        }
        public async Task<bool>MarkAsReturnedAsync(int foundItemId,int userId)
        {
            var foundItem = await _Context.FoundItems.FirstOrDefaultAsync(x=> x.Id==foundItemId);
            if (foundItem == null)
            
                return false;
            if(foundItem.UserId !=userId)
            return false;

            if(foundItem.IsReturned)
            return false;

            foundItem.IsReturned=true;
            foundItem.ReturnedAt=DateTime.UtcNow;

            await _Context.SaveChangesAsync();
            return true;


        }
        
        }
}