namespace WarehouseMagementApi.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int StorageZoneId { get; set; }
    public StorageZone StorageZone { get; set; } = null!;
}