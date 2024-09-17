using AutoMonitoring.BLL.DTOs.Implementations.Responses.Role;

namespace AutoMonitoring.BLL.Services.Interfaces;

public interface IRoleService
{
    Task<IEnumerable<RoleDTO>> GetRolesByUserIdAsync(Guid userId, CancellationToken cancellationToken=default);
}