namespace WarehouseManagementApi.DTOs;

public class ProductDto
{
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int StorageZoneId { get; set; }
}
