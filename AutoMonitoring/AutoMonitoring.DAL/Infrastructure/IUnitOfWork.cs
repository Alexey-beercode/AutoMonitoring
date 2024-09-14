using AutoMonitoring.DAL.Repositories.Interfaces;

namespace AutoMonitoring.DAL.Infrastructure;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken=default);
    IUserSessionRepository UserSessions { get; }
    IUserRepository Users { get; }
    IRoleRepository Roles { get; }
}