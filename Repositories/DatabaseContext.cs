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
            modelBuilder.Entity<SMSVerification>(e =>
            {
                e.HasOne(v => v.User).WithOne(u => u.Verification).HasForeignKey<SMSVerification>(v => v.UserId);
            });
            modelBuilder.Entity<SubmittedFactor>(c =>
            {
                c.HasOne(v => v.Contact).WithOne(u => u.SubmittedFactor).HasForeignKey<SubmittedFactor>(v => v.ContactId);
            });
            modelBuilder.Entity<PreFactor>(p =>
            {
                p.HasOne(v => v.SubmittedFactor).WithOne(u => u.PreFactor).HasForeignKey<PreFactor>(v => v.SubmittedFactorId);
            });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<SMSVerification> Verifications { get; set; }
        public DbSet<PreFactor> Factors { get; set; }
    }
}
