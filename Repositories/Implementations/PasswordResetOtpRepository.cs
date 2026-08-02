using LostAndFoundAPI.Data;
using LostAndFoundAPI.Models;
using LostAndFoundAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LostAndFoundAPI.Repositories.Implementations
{
    public class PasswordResetOtpRepository : IPasswordResetOtpRepository
    {
        private readonly AppDbContext _context;
        public PasswordResetOtpRepository(AppDbContext context)
        {
            _context=context;
        }

        public async Task DeleteOtpAsync(PasswordResetOtp otp)
        {
            _context.PasswordResetOtps.Remove(otp);
            await _context.SaveChangesAsync();
        }

        public async Task<PasswordResetOtp?> GetOtpByUserIdAsync(int userId)
        {
            return await _context.PasswordResetOtps.FirstOrDefaultAsync(x=> x.UserId ==userId);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
           return await _context.Users.FirstOrDefaultAsync(x=> x.Email == email);
        }

        public async Task UpsertOtpAsync(PasswordResetOtp otp)
        {
            var existingOtp = await GetOtpByUserIdAsync(otp.UserId);
            if (existingOtp ==null)
            {
                _context.PasswordResetOtps.Add(otp);
                
            }
            else
            {
                existingOtp.OTP =otp.OTP;
                existingOtp.ExpiryTime=otp.ExpiryTime;
                existingOtp.CreatedAt=otp.CreatedAt;
                existingOtp.IsVerified= false;
            }

              
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            

            await _context.SaveChangesAsync();
        }
    }

}