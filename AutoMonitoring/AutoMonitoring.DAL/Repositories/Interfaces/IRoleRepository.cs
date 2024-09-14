using AutoMonitoring.Domain.Entities.Implementations;

namespace AutoMonitoring.DAL.Repositories.Interfaces;

public interface IRoleRepository:IBaseRepository<Role>
{
    Task<Role> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Role>> GetRolesByUserIdAsync(Guid userId, CancellationToken cancellationToken=default);
}