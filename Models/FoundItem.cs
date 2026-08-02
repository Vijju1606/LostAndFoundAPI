using System.Security.Cryptography;

namespace LostAndFoundAPI.Models;
public class FoundItem
{
    public int Id { get; set; }
    public string ItemName { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public DateTime DateFound { get; set; }
    public int UserId { get; set; }
    public string ImageUrl { get; set; }
    public bool IsReturned{get; set;}=false;
    public DateTime? ReturnedAt{get; set;}
}