using LostAndFoundAPI.Common;
using LostAndFoundAPI.Models;
using LostAndFoundAPI.Services.Interfaces;
using LostAndFoundAPI.Repositories.Interfaces;

namespace LostAndFoundAPI.Services.Implementations
{
    public class LostItemService : ILostItemService
    {
        private readonly ILostItemRepository _repository;
        private readonly IFileService _fileService;

        public LostItemService(ILostItemRepository repository , IFileService fileService)
        {
            _repository = repository;
            _fileService=fileService;
        }








        public async Task<ApiResponse> CreateLostItem(CreateLostItemDto dto, int userId)
        {
            if (dto.Image != null)
            {
                var uploadResult = await _fileService.UploadImageAsync(dto.Image);

                 if(!uploadResult.Success)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = uploadResult.Message
                };
            }
            dto.ImageUrl=uploadResult.ImageUrl;

            }

           



            return await _repository.CreateLostItem(dto, userId);
        }

        public ApiResponse DeleteLostItem(int id, int userId)
        {
            return _repository.DeleteLostItem(id, userId);
        }

        public LostItem? GetLostItemById(int id, int userId)
        {
            return _repository.GetLostItemById(id, userId);
        }

        public ApiResponse GetMyLostItems(int userId)
        {
            return _repository.GetMyLostItems(userId);
        }
        

        public async Task< ApiResponse> UpdateLostItem(int id, CreateLostItemDto dto, int userId)
        {
            return await _repository.UpdateLostItem(id,dto,userId);
        }
        public async Task<ApiResponse> GetAllLostItemsAsync()
        {
           var response =await _repository.GetAllLostItemsAsync();
            return new ApiResponse
            {
              Success=true,
              Message="Lost items retrieved Successfully"  ,
              Data=response
            };
        }

        public async Task<ApiResponse> GetByIdAsync(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if(item == null)
            {
                return new ApiResponse
                {
                    Success=false,
                    Message="Lost item not found."
                };
            }
            return new ApiResponse
            {
                Success=true,
                Message="Lost item retrieved successfully.",
                Data=item
            };
        }

    }
}