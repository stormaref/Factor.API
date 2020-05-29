using Factor.Models;
using Microsoft.EntityFrameworkCore;

namespace Factor.Repositories
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<SMSVerification>(). HasOne(v => v.User).WithOne(u => u.Verification).HasForeignKey<SMSVerification>(v=>v.UserId);
            //modelBuilder.Entity<SMSVerification>().HasKey(v => v.UserId);
            //modelBuilder.Entity<SMSVerification>().HasOne(v => v.User).WithOne(u => u.Verification).HasForeignKey<SMSVerification>(v => v.UserId);
            modelBuilder.Entity<SMSVerification>(e =>
            {
                e.HasOne(v => v.User).WithOne(u => u.Verification).HasForeignKey<SMSVerification>(v => v.UserId);
            });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<SMSVerification> Verifications { get; set; }
        public DbSet<Models.Factor> Factors { get; set; }
    }
}
