using WarehouseManagementApi.Models;

namespace WarehouseManagementApi.DTOs;

public class ProductDto
{
    public ProductDto(Product product)
    {
        Id = product.Id;
        Name = product.Name;
        Quantity = product.Quantity;
        StorageZone = new StorageZoneDto(product.StorageZone);
    }

    public ProductDto() { }

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public StorageZoneDto StorageZone { get; set; }
}
