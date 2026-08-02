using LostAndFoundAPI.Models;
using Microsoft.AspNetCore.Http;

public interface IFileService
{
    Task<FileUploadResult> UploadImageAsync(IFormFile file);
    
}