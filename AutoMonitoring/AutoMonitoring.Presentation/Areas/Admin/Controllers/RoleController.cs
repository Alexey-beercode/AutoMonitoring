using AutoMonitoring.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoMonitoring.Areas.Admin.Controllers;

[Area("admin")]
[Authorize("AdminArea")]
[Route("api/admin/role")]
[ApiController]
public class RoleController:ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet("getByUserId{userId}")]
    public async Task<IActionResult> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var roles = await _roleService.GetRolesByUserIdAsync(userId, cancellationToken);
        return Ok(roles);
    }
}