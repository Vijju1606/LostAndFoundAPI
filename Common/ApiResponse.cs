namespace LostAndFoundAPI.Common
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public string? Token { get; set; }
        public object? Data { get; set; }
        
    }


}