using AutoMonitoring.BLL.DTOs.Implementations.Responses.Role;
using AutoMonitoring.Domain.Models;

namespace AutoMonitoring.BLL.Services.Interfaces;

public interface IRoleService
{
    Task<IEnumerable<RoleDTO>> GetRolesByUserIdAsync(Guid userId, CancellationToken cancellationToken=default);
    Task SetRoleToUserAsync(UserRoleModel userRole, CancellationToken cancellationToken=default);
    
}