using AutoMonitoring.DAL.Infrastructure.Database;
using AutoMonitoring.DAL.Repositories.Interfaces;
using AutoMonitoring.Domain.Entities.Implementations;
using Microsoft.EntityFrameworkCore;

namespace AutoMonitoring.DAL.Repositories.Implementations;

public class RoleRepository:BaseRepository<Role>,IRoleRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public RoleRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Role> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == name && !r.IsDeleted);
    }

    public async Task<IEnumerable<Role>> GetRolesByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var roles = await (from userRole in _dbContext.UserRoles
            join role in _dbContext.Roles on userRole.RoleId equals role.Id
            where userRole.UserId == userId
            select role).ToListAsync(cancellationToken);

        return roles;
    }
}