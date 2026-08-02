using System.ComponentModel.DataAnnotations;

public class VerifyOtpDto
{
    [Required]
    [EmailAddress]
    public string Email{get; set;} = string.Empty;
    [Required]
    [StringLength(6,MinimumLength =6)]
    public string OTP{get; set;}= string.Empty;
}