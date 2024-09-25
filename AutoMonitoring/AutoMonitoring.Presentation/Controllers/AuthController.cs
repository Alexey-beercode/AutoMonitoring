using AutoMonitoring.BLL.DTOs.Implementations.Requests.Token;
using AutoMonitoring.BLL.DTOs.Implementations.Requests.User;
using AutoMonitoring.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutoMonitoring.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController:ControllerBase
{
    private readonly IUserSessionService _userSessionService;
    private readonly IUserService _userService;

    public AuthController(IUserSessionService userSessionService, IUserService userService)
    {
        _userSessionService = userSessionService;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDTO loginDto, CancellationToken cancellationToken = default)
    {
        var token = await _userService.LoginAsync(loginDto, cancellationToken);
        return Ok(token);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenDTO refreshTokenDto,
        CancellationToken cancellationToken = default)
    {
        var token = await _userService.RefreshTokenAsync(refreshTokenDto, cancellationToken);
        return Ok(token);
    }

    [HttpPut("logout/{userId}")]
    public async Task<IActionResult> RevokeAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        await _userSessionService.RevokeSessionAsync(userId, cancellationToken);
        return Ok();
    }
}