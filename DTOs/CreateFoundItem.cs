public class CreateFoundItemDto
{
    public string ItemName { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }

    public DateTime DateFound { get; set; }
    public IFormFile? Image { get; set; }
    public string? ImageUrl{get; set;}
}