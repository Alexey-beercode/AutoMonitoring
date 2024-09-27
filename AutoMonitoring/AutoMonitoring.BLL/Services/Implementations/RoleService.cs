using AutoMonitoring.BLL.DTOs.Implementations.Responses.Role;
using AutoMonitoring.BLL.Services.Interfaces;
using AutoMonitoring.DAL.Infrastructure;
using AutoMonitoring.Domain.Models;

namespace AutoMonitoring.BLL.Services.Implementations;

public class RoleService:IRoleService
{
    private readonly IUnitOfWork _unitOfWork;

    public RoleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        
    }

    public Task<IEnumerable<RoleDTO>> GetRolesByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task SetRoleToUserAsync(UserRoleModel userRole, CancellationToken cancellationToken = default)
    {
        var isSuccess = await _unitOfWork.Roles.SetRoleToUserAsync(userRole.UserId, userRole.RoleId, cancellationToken);
        if (!isSuccess)
        {
            throw new InvalidOperationException($"Role with id : {userRole.RoleId} cannot be set to user with id : {userRole.UserId}");
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}