using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementApi.Data;
using WarehouseManagementApi.DTOs;
using WarehouseManagementApi.Models;

namespace WarehouseManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrator")]
public class AdminController : ControllerBase
{
    private readonly WarehousesDbContext _context;

    public AdminController(WarehousesDbContext context)
    {
        _context = context;
    }

    [HttpPost("register-storekeeper")]
    [ProducesResponseType<BadRequestObjectResult>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<OkObjectResult>(StatusCodes.Status200OK)]
    public async Task<IActionResult> RegisterStorekeeper([FromBody] RegisterDto registerDto)
    {
        if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
            return BadRequest(new { message = "Username already exists" });

        var user = new User {
            Username = registerDto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
            Role = "Storekeeper"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Storekeeper registered successfully" });
    }

    [HttpDelete("delete-storekeeper/{id}")]
    [ProducesResponseType<NotFoundResult>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<BadRequestObjectResult>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<OkResult>(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteStorekeeper(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return NotFound();

        if (user.Role != "Storekeeper")
            return BadRequest(new { message = "Only storekeepers can be deleted through this endpoint" });

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("add-warehouse")]
    public async Task<IActionResult> AddWarehouse([FromBody] WarehouseDto warehouseDto)
    {
        var warehouse = new Warehouse {
            Name = warehouseDto.Name,
            Address = warehouseDto.Address,
        };

        _context.Warehouses.Add(warehouse);
        await _context.SaveChangesAsync();

        return Ok(warehouse);
    }

    [HttpDelete("delete-warehouse/{id}")]
    [ProducesResponseType<NotFoundResult>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<OkResult>(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteWarehouse(int id)
    {
        var warehouse = await _context.Warehouses.FindAsync(id);

        if (warehouse == null)
            return NotFound();

        _context.Warehouses.Remove(warehouse);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("storekeepers")]
    public async Task<ActionResult<List<UserDto>>> GetAllStorekeepers()
    {
        var storekeepers = await _context.Users
            .Where(u => u.Role == "Storekeeper")
            .Select(u => new UserDto {
                Id = u.Id,
                Username = u.Username
            })
            .ToListAsync();

        return Ok(storekeepers);
    }
}
