using LostAndFoundAPI.Repositories.Interfaces;
using LostAndFoundAPI.Services.Interfaces;
using LostAndFoundAPI.Common;
using LostAndFoundAPI.DTOs;
using LostAndFoundAPI.Models;
using System.Reflection.Metadata.Ecma335;


namespace LostAndFoundAPI.Services.Implementations
{
    public class FoundItemService : IFoundItemService{
    private readonly IFoundItemRepository _repository;
    private readonly IFileService _fileService;

    public FoundItemService(IFoundItemRepository repository,IFileService fileService)
    {
        _repository = repository;
        _fileService = fileService;
        
    }

        public async Task<ApiResponse> CreateFoundItem(CreateFoundItemDto dto, int userId)
        {
            if(dto.Image != null)
            {
                var uploadresult = await _fileService.UploadImageAsync(dto.Image);
                if(!uploadresult.Success)
                {
                    return new ApiResponse
                    {
                        Success=false,
                        Message = uploadresult.Message
                    };
                } 
                dto.ImageUrl= uploadresult.ImageUrl;
            }
            return await _repository.CreateFoundItem(dto,userId);

        }

        public ApiResponse DeleteFoundItem(int id, int userId)
        {
            return _repository.DeleteFoundItem(id,userId);
        }

        public List<FoundItem> GetAllFoundItems()
        {
            return _repository.GetAllFoundItems();
        }

        public ApiResponse GetMyFoundItems(int userId)
        {
            return _repository.GetMyFoundItems(userId);
        }

        public ApiResponse UpdateFoundItem(int id, CreateFoundItemDto dto, int userId)
        {
            return _repository.UpdateFoundItem(id,dto,userId);
        }

        public async Task<ApiResponse>MarkAsReturnedAsync(int foundItemId, int userId)
        {
        var result= await _repository.MarkAsReturnedAsync(foundItemId,userId);
            if (!result)
            {
                return new ApiResponse
                {
                    Success=false,
                    Message="Item not Found,or Already returned or You are not authorized."
                };
            }
            return new ApiResponse
            {
                Success=true,
                Message="Item Marked As returned successfully."
            };
        }


        public async Task<ApiResponse>GetByIdAsync(int id)
        {
            var foundItem = await _repository.GetByIdAsync(id);
            if (foundItem == null)
            {
                return new ApiResponse
                {
                    Success= false,
                    Message="Found Item not found. "
                };

            }
            return new ApiResponse
            {
                Success=true,
                Message="Found item Retrieved successfully.",
                Data=foundItem
            };
        }
    }
}