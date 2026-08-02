namespace LostAndFoundAPI.DTOs
{
    public class AdminFoundItemDto
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
         public string Location { get; set; }
         public DateTime DateFound { get; set; }
          public string ImageUrl { get; set; }
           public bool IsReturned { get; set; }
         public string OwnerName { get; set; }
       public string OwnerEmail { get; set; }
    }
}