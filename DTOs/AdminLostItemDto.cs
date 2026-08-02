namespace LostAndFoundAPI.DTOs
{
    public class AdminLostItemDto
    {
        public int Id{get; set;}
        public string ItemName{get; set;}=string.Empty;
        public string Description{get; set;}=string.Empty;
        public string Location{get; set;}=string.Empty;
        public DateTime DateLost{get;set;}
        public string ImageUrl{get; set;}=string.Empty;
        public string OwnerName{get; set;}=string.Empty;
        public string OwnerEmail{get; set;}=string.Empty;
        
    
    }
}