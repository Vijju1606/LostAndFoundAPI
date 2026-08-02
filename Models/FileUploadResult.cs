namespace LostAndFoundAPI.Models
{
    public class FileUploadResult
    {
        public bool Success { get; set; }
        public string? ImageUrl { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}