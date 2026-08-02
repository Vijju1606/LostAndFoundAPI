namespace LostAndFoundAPI.Models;
public class LostItem
{
    public int Id { get; set; }
    public string ItemName { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public DateTime DateLost { get; set; }
    public int UserId { get; set; }
    public string ImageUrl { get; set; }
}