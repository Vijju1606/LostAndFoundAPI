using LostAndFoundAPI.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using LostAndFoundAPI.Common;

namespace LostAndFoundAPI.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly Cloudinary _cloudinary;

public FileService(IOptions<CloudinarySettings> settings)
{
    var account = new Account(
        settings.Value.CloudName,
        settings.Value.ApiKey,
        settings.Value.ApiSecret);

    _cloudinary = new Cloudinary(account);
}

        public async Task<FileUploadResult> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return new FileUploadResult
                {
                    Success = false,
                    Message = "Please select a valid image file."
                };
            }
            var extension = Path.GetExtension(file.FileName).ToLower();
            var allowedExtensions = new[]
            {
                ".jpg", ".jpeg", ".png", ".gif"
            };
            if (!allowedExtensions.Contains(extension))
            {
                return new FileUploadResult
                {
                    Success = false,
                    Message = "Only image files (jpg, jpeg, png, gif) are allowed."
                };

            }

            if (file.Length > 5 * 1024 * 1024)
            {
                return new FileUploadResult
                {
                    Success = false,
                    Message = "File size exceeds the 5MB limit."
                };
            }

            try{

           using var stream = file.OpenReadStream();

var uploadParams = new ImageUploadParams
{
    File = new FileDescription(file.FileName, stream),
    Folder = "LostAndFound"
};

var uploadResult = await _cloudinary.UploadAsync(uploadParams);

if (uploadResult.Error != null)
{
    return new FileUploadResult
    {
        Success = false,
        Message = uploadResult.Error.Message
    };
}

return new FileUploadResult
{
    Success = true,
    ImageUrl = uploadResult.SecureUrl.ToString(),
    Message = "Image uploaded successfully."
};
              }
            catch (Exception ex)
            {
                return new FileUploadResult
                {
                    Success = false,
                    Message = ex.Message
                };
            }
            
            
        }
    }
}