using Microsoft.EntityFrameworkCore;
using WarehouseMagementApi.Models;

namespace WarehouseMagementApi.Data;

public class WarehousesDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Warehouse> Warehouses { get; set; } = null!;
    public DbSet<StorageZone> StorageZones { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;

    public WarehousesDbContext(DbContextOptions<WarehousesDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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