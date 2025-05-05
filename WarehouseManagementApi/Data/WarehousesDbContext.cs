using Microsoft.EntityFrameworkCore;
using WarehouseManagementApi.Models;

namespace WarehouseManagementApi.Data;

public class WarehousesDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Warehouse> Warehouses { get; set; } = null!;
    public DbSet<StorageZone> StorageZones { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;

    public WarehousesDbContext(DbContextOptions<WarehousesDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var adminUser = new User {
            Id = 1,
            Username = "admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"),
            Role = "Administrator"
        };

        modelBuilder.Entity<User>().HasData(adminUser);

        modelBuilder.Entity<StorageZone>()
            .HasOne(sz => sz.Warehouse)
            .WithMany(w => w.StorageZones)
            .HasForeignKey(sz => sz.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.StorageZone)
            .WithMany(sz => sz.Products)
            .HasForeignKey(p => p.StorageZoneId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}