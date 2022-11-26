using Microsoft.EntityFrameworkCore;
using StarBucks.Domain.Entities;

namespace StarBucks.Data.Contexts
{
    public class StarbucksDbContext : DbContext
    {
        public virtual DbSet<Coffee> Cofees { get; set; }
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public StarbucksDbContext(
            DbContextOptions<StarbucksDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Address)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
        }
    }
}
