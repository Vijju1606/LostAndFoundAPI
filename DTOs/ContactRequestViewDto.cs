public class ContactRequestViewDto
{
    public int ContactRequestId{get; set;}
    public string LostItemName{get; set;}=string.Empty;
    public string FoundItemName{get; set;}=string.Empty;
    public string RequestedBy{get; set;}=string.Empty;
    public string RequestedTo{get; set;}=string.Empty;
    public string Status{get; set;}=string.Empty;
    public string? sharedPhoneNumber{get; set;}
    public DateTime RequestedAt{get; set;}
    public DateTime? RespondedAt{get; set;}

}