namespace WarehouseMagementApi.Models;

public class StorageZone
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = null!;
    public virtual ICollection<Product> Products { get; set; } = [];
}

