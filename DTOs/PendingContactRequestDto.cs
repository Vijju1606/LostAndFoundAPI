namespace LostAndFoundAPI.DTOs{
public class PendingContactRequestDto
{
    public int ContactRequestId{get; set;}
    public int FoundItemId{get; set;}
   
    public string LostItemTitle{get; set;}=string.Empty;
    public string LostItemDescription{get; set;}=string.Empty;
    public string FoundItemTitle{get; set;}=string.Empty;
    public string FoundItemDescription{get; set;}=string.Empty;
    public int? MatchScore{get; set;}
    public string RequestedByName{get; set;}= string.Empty;
    public string RequestedByEmail{get; set;}= string.Empty;
    public DateTime RequestedAt{get;set;}
    public string Status { get; set;}=string.Empty;

}
}
