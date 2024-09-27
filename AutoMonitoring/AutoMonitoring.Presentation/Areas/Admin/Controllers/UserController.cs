using AutoMonitoring.BLL.DTOs.Implementations.Requests.User;
using AutoMonitoring.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoMonitoring.Areas.Admin.Controllers;

[Area("admin")]
[Authorize("AdminArea")]
[Route("api/admin/user")]
[ApiController]
public class UserController:ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateUserAsync(UserDTO userDto, CancellationToken cancellationToken = default)
    {
        await _userService.RegisterAsync(userDto, cancellationToken);
        return Ok();
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userService.GetAllAsync(cancellationToken);
        return Ok(users);
    }

    [HttpPut("block")]
    public async Task<IActionResult> BlockUserAsync(BlockUserDTO blockUserDto, CancellationToken cancellationToken = default)
    {
        await _userService.BlockUserAsync(blockUserDto, cancellationToken);
        return Ok();
    }
}