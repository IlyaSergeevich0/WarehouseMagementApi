using System.ComponentModel.DataAnnotations;

namespace WarehouseManagementApi.DTOs;

public class ProductDto
{
    public ProductDto() { }

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    [Required] public int StorageZoneId { get; set; }
}
