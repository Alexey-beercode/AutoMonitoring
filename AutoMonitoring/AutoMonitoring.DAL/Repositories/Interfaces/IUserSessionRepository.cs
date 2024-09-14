using AutoMonitoring.Domain.Entities.Implementations;

namespace AutoMonitoring.DAL.Repositories.Interfaces;

public interface IUserSessionRepository:IBaseRepository<UserSession>
{
    Task<UserSession> GetActiveSessionAsync(Guid userId,CancellationToken cancellationToken=default);
    void Update(UserSession userSession);
}