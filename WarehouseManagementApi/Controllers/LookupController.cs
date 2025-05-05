using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementApi.Data;
using WarehouseManagementApi.DTOs;

namespace WarehouseManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Storekeeper,Administrator")]
public class LookupController : ControllerBase
{
    private readonly WarehousesDbContext _context;

    public LookupController(WarehousesDbContext context)
    {
        _context = context;
    }

    [HttpGet("products")]
    public async Task<ActionResult<List<ProductDto>>> GetProducts()
    {
        var products = await _context.Products.Select(p => new ProductDto {
            Name = p.Name,
            Quantity = p.Quantity
        }).ToListAsync();

        return Ok(products);
    }

    [HttpGet("storage-zones")]
    public async Task<ActionResult<List<StorageZoneDto>>> GetStorageZones()
    {
        var storageZones = await _context.StorageZones.Select(s => new StorageZoneDto {
            Name = s.Name,
            WarehouseId = s.WarehouseId
        }).ToListAsync();

        return Ok(storageZones);
    }

    [HttpGet("warehouses")]
    public async Task<ActionResult<List<WarehouseDto>>> GetWarehouses()
    {
        var warehouses = await _context.Warehouses
            .Select(w => new WarehouseDto {
                Id = w.Id,
                Name = w.Name,
                Address = w.Address
            })
            .ToListAsync();

        return Ok(warehouses);
    }
}
