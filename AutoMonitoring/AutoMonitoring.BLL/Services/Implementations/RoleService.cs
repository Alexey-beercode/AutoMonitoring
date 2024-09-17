using AutoMonitoring.BLL.DTOs.Implementations.Responses.Role;
using AutoMonitoring.BLL.Services.Interfaces;
using AutoMonitoring.DAL.Infrastructure;

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
}