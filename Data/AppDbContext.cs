using Microsoft.EntityFrameworkCore;
using LostAndFoundAPI.Models;
namespace LostAndFoundAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<LostItem> LostItems { get; set; }
        public DbSet<FoundItem> FoundItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PasswordResetOtp> PasswordResetOtps{get; set;}
        public DbSet<ContactRequest>ContactRequests{get; set;}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ContactRequest>() .HasOne(cr => cr.LostItem).WithMany().HasForeignKey(cr=> cr.LostItemId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ContactRequest>() .HasOne(cr => cr.FoundItem).WithMany().HasForeignKey(cr=> cr.FoundItemId).OnDelete(DeleteBehavior.Restrict);
             modelBuilder.Entity<ContactRequest>() .HasOne(cr => cr.RequestedByUser).WithMany().HasForeignKey(cr=> cr.RequestedByUserId).OnDelete(DeleteBehavior.Restrict);
              modelBuilder.Entity<ContactRequest>() .HasOne(cr => cr.RequestedToUser).WithMany().HasForeignKey(cr=> cr.RequestedToUserId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}