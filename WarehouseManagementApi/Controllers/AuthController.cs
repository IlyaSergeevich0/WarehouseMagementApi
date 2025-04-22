using Microsoft.AspNetCore.Mvc;
using WarehouseManagementApi.DTOs;
using WarehouseManagementApi.Services;

namespace WarehouseManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await _authService.ValidateUser(loginDto.Username, loginDto.Password);

        if (user is null)
            return Unauthorized();

        var jwtToken = _authService.GenerateJwtToken(user);

        return Ok(new AuthResponseDto {
            Token = jwtToken,
            Role = user.Role
        });
    }
}
