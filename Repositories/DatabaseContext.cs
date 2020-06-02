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
            modelBuilder.Entity<SMSVerification>(e =>
            {
                e.HasOne(v => v.User).WithOne(u => u.Verification).HasForeignKey<SMSVerification>(v => v.UserId);
            });
            modelBuilder.Entity<Contact>(c =>
            {
                c.HasOne(v => v.SubmittedFactor).WithOne(u => u.Contact).HasForeignKey<Contact>(v => v.SubmitedFactorId);
            });
            modelBuilder.Entity<PreFactor>(p =>
            {
                p.HasOne(v => v.SubmittedFactor).WithOne(u => u.PreFactor).HasForeignKey<PreFactor>(v => v.SubmittedFactorId);
            });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<SMSVerification> Verifications { get; set; }
        public DbSet<PreFactor> Factors { get; set; }
    }
}
