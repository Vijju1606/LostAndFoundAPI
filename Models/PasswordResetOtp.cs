using System;

namespace LostAndFoundAPI.Models
{
    public class PasswordResetOtp
    {
    public int Id {get; set;}
    public int UserId {get; set;}
    public User? User {get; set;}
    public string OTP {get; set;}= string.Empty;
    public DateTime ExpiryTime{get; set;}
    public DateTime CreatedAt{get; set;}
    public bool IsVerified{get; set;}

    }
}