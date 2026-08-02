using LostAndFoundAPI.Models;


namespace LostAndFoundAPI.Repositories.Interfaces
{
    public interface IPasswordResetOtpRepository{
    Task<User?>GetUserByEmailAsync(string email);
    Task<PasswordResetOtp?>GetOtpByUserIdAsync(int userId);
    Task UpsertOtpAsync(PasswordResetOtp otp);
    Task DeleteOtpAsync(PasswordResetOtp otp);
    
    Task SaveChangesAsync();

}
}