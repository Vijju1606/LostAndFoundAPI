public class MatchResultDto
{
    public int FoundItemId { get; set; }
    public string ItemName { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public DateTime DateFound { get; set; }

    public int MatchPercentage { get; set; }
    public string? ImageUrl { get; set; }
   
}