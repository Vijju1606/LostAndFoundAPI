public class CreateLostItemDto
{
    public string ItemName { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }

    public DateTime DateLost { get; set; }
  
    public IFormFile? Image { get; set; } 
    public string? ImageUrl{ get; set;}

}