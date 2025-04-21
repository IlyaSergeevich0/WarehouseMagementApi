namespace WarehouseManagementApi.Models;

public class Warehouse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public virtual ICollection<StorageZone> StorageZones { get; set; } = [];
}