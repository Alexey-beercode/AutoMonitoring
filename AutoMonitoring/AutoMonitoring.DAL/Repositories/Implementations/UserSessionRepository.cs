using AutoMonitoring.DAL.Infrastructure.Database;
using AutoMonitoring.DAL.Repositories.Interfaces;
using AutoMonitoring.Domain.Entities.Implementations;
using Microsoft.EntityFrameworkCore;

namespace AutoMonitoring.DAL.Repositories.Implementations;

public class UserSessionRepository:BaseRepository<UserSession>,IUserSessionRepository
{
    private readonly ApplicationDbContext _dbContext;
    public UserSessionRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserSession> GetActiveSessionByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserSessions
            .FirstOrDefaultAsync(s => s.UserId == userId && s.IsActive && !s.IsDeleted);
    }

    public async Task<IEnumerable<UserSession>> GetAllSessionsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserSessions.Where(us => !us.IsDeleted).ToListAsync(cancellationToken);
    }

    public void Update(UserSession userSession)
    {
        _dbContext.UserSessions.Update(userSession);
    }
}