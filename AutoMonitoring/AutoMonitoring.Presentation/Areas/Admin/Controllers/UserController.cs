using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoMonitoring.Areas.Admin.Controllers;

[Area("admin")]
[Authorize("AdminArea")]
[Route("api/admin/user")]
[ApiController]
public class UserController:ControllerBase
{
    
}