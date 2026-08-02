
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
    public class LostItemRepository : ILostItemRepository
    {
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;

        public LostItemRepository(AppDbContext context , IFileService fileService)
        {
            _context = context;
            _fileService=fileService;
        }

        
        public async Task< ApiResponse> CreateLostItem(CreateLostItemDto dto, int userId)
        {
            var lostItem = new LostItem();
            lostItem.ItemName = dto.ItemName;
            lostItem.Description = dto.Description;
            lostItem.Location = dto.Location;
            lostItem.DateLost = dto.DateLost;
            lostItem.ImageUrl=dto.ImageUrl;
            lostItem.UserId = userId;

            _context.LostItems.Add(lostItem);
            _context.SaveChanges();

            return new ApiResponse
            {
                Success = true,
                Message = "Lost item created successfully.",
                Data = lostItem
            };
            
            
        }



        public ApiResponse GetMyLostItems(int userId)
        {
            var lostItems = _context.LostItems.Where(x=> x.UserId == userId).ToList();

            return new ApiResponse
            {
                Success = true,
                Message = "Lost items retrieved successfully.",
                Data = lostItems
            };
        }


        public async Task<ApiResponse> UpdateLostItem(int id, CreateLostItemDto dto, int userId)
        {
            var lostItem = _context.LostItems.FirstOrDefault(x => x.Id== id);
               if (lostItem == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Lost item not found."
                };

            }

            if (lostItem.UserId != userId)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "You are not authorized to update this lost item."
                };
            }
            lostItem.ItemName = dto.ItemName;
            lostItem.Description = dto.Description;
            lostItem.Location = dto.Location;
            lostItem.DateLost = dto.DateLost;


             if(dto.Image != null)
            {
                var uploadResult = await _fileService.UploadImageAsync(dto.Image);

                if (!uploadResult.Success)
                {
                    return new ApiResponse
                    {
                        Success=false,
                        Message= uploadResult.Message

                    };
                }
                lostItem.ImageUrl=uploadResult.ImageUrl;
            }

            await _context.SaveChangesAsync();
            return new ApiResponse
            {
                Success = true,
                Message = "Lost item updated successfully."
            };
        }

        public ApiResponse DeleteLostItem(int id, int userId)
        {
            var lostItem = _context.LostItems.FirstOrDefault(x=> x.Id == id);
            if (lostItem == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Lost item not found."
                };
            }

            if (lostItem.UserId != userId)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "You are not authorized to delete this lost item."
                };
            }

            _context.LostItems.Remove(lostItem);
            _context.SaveChanges();
            return new ApiResponse
            {
                Success = true,
                Message = "Lost item deleted successfully."
            };
        }

        public LostItem? GetLostItemById(int id, int userId)
        {
            return _context.LostItems.FirstOrDefault(x => x.Id == id && x.UserId == userId);
        }

     public async Task<LostItem?>GetByIdAsync(int id)
        {
            return await _context.LostItems.FirstOrDefaultAsync(x=>x.Id ==id);
        }

       

        public async Task<List<LostItem>> GetAllLostItemsAsync()
        {
            return await _context.LostItems.OrderByDescending(x=> x.DateLost).ToListAsync();
        }
        
    }
}
