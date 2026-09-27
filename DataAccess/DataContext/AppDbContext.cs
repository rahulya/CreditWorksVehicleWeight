using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataContext
{
    public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<Manufacturer> Manufacturers => Set<Manufacturer>();
        public DbSet<VehicleCategory> VehicleCategories => Set<VehicleCategory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.Property(x => x.Name).HasMaxLength(80).IsRequired();
                entity.HasIndex(x => x.Name).IsUnique();
            });
            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.Property(x => x.OwnerName).HasMaxLength(120).IsRequired();
                entity.Property(x => x.WeightKg).HasPrecision(10, 2);
                entity.HasIndex(x => x.WeightKg);
                entity.HasOne(x => x.Manufacturer).WithMany(x => x.Vehicles)
                    .HasForeignKey(x => x.ManufacturerId).OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<VehicleCategory>(entity =>
            {
                entity.Property(x => x.Name).HasMaxLength(80).IsRequired();
                entity.Property(x => x.MinimumWeightKg).HasPrecision(10, 2);
                entity.Property(x => x.MaximumWeightKg).HasPrecision(10, 2);
                entity.Property(x => x.Icon).HasMaxLength(20).IsRequired();
            });
        }
    }
}
