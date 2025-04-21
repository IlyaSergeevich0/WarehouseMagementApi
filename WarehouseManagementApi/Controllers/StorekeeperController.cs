using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagementApi.Data;
using WarehouseManagementApi.Models;

namespace WarehouseManagementApi.Controllers;

[Authorize(Roles = "Storekeeper")]
[ApiController]
[Route("api/[controller]")]
public class StorekeeperController : ControllerBase
{
    private readonly WarehousesDbContext _context;

    public StorekeeperController(WarehousesDbContext context)
    {
        _context = context;
    }

    [HttpPost("add-product")]
    public async Task<IActionResult> AddProduct([FromBody] Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return Ok(product);
    }

    [HttpDelete("remove-product/{id}")]
    public async Task<IActionResult> RemoveProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("add-storage-zone")]
    public async Task<IActionResult> AddStorageZone([FromBody] StorageZone storageZone)
    {
        _context.StorageZones.Add(storageZone);
        await _context.SaveChangesAsync();

        return Ok(storageZone);
    }

    [HttpDelete("delete-storage-zone/{id}")]
    public async Task<IActionResult> DeleteStorageZone(int id)
    {
        var storageZone = await _context.StorageZones.FindAsync(id);

        if (storageZone == null)
            return NotFound();

        _context.StorageZones.Remove(storageZone);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("move-product")]
    public async Task<IActionResult> MoveProduct(int productId, int targetStorageZoneId)
    {
        var product = await _context.Products.FindAsync(productId);

        if (product == null)
            return NotFound("Product not found");

        var targetZone = await _context.StorageZones.FindAsync(targetStorageZoneId);

        if (targetZone == null)
            return NotFound("Target storage zone not found");

        product.StorageZoneId = targetStorageZoneId;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Product moved successfully" });
    }
}
