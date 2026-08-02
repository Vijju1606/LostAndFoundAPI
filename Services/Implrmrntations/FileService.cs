using LostAndFoundAPI.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace LostAndFoundAPI.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
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
            var originalFileName = Path.GetFileName(file.FileName);
            var fileName = $"{Guid.NewGuid()}_{originalFileName}";

           var webRoot = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(),"wwwroot");
            var folderPath = Path.Combine(webRoot, "Images");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, fileName);

            try{

            using var stream = new FileStream(filePath,FileMode.Create);
            await file.CopyToAsync(stream);


            return new FileUploadResult
            {
                Success = true,
                ImageUrl = $"/Images/{fileName}",
                Message = "Image uploaded successfully."
            };  }
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