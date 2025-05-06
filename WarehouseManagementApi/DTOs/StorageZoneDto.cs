using System.ComponentModel.DataAnnotations;
using WarehouseManagementApi.Models;

namespace WarehouseManagementApi.DTOs;

public class StorageZoneDto
{
    public StorageZoneDto(StorageZone storageZone)
    {
        Id = storageZone.Id;
        Name = storageZone.Name;
    }

    public StorageZoneDto() { }

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    [Required] public int WarehouseId { get; set; }
}

