using AutoMonitoring.DAL.Infrastructure.Database;
using AutoMonitoring.DAL.Repositories.Interfaces;
using AutoMonitoring.Domain.Entities.Implementations;
using Microsoft.EntityFrameworkCore;

namespace AutoMonitoring.DAL.Repositories.Implementations;

public class UserRepository:BaseRepository<User>,IUserRepository
{
    private new readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> GetByLoginAsync(string login, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Login == login && !u.IsDeleted, cancellationToken);
    }

    public void Update(User user)
    {
        _dbContext.Users.Update(user);
    }

    public void Delete(User entity)
    {
        base.Delete(entity);
        var usersRolesByUser =  _dbContext.UserRoles.Where(ur => ur.UserId == entity.Id);
        foreach (var userRole in usersRolesByUser)
        {
            userRole.IsDeleted = true;
            _dbContext.UserRoles.Update(userRole);
        }
    }
}