using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LostAndFoundAPI.Models
{
    public class ContactRequest
    {
        [Key]
        public int ContactRequestId { get; set; }

        public int? LostItemId { get; set; }

        

        [Required]
        public int FoundItemId { get; set; }

       

        [Required]
        public int RequestedByUserId { get; set; }

       

        [Required]
        public int RequestedToUserId { get; set; }

        

        [Required]
        public string Status { get; set; } = "Pending";

        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

        public DateTime? RespondedAt { get; set; }

        public int? MatchScore{get; set;}
        public string? SharedPhoneNumber{get; set;}

        public LostItem? LostItem{get; set;}
        public FoundItem FoundItem{get; set;}=null!;
        public User RequestedByUser{get; set;}=null!;
        public User RequestedToUser{get; set;}=null!;
    }
}
