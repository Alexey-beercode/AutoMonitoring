using AutoMonitoring.Domain.Entities.Implementations;

namespace AutoMonitoring.DAL.Repositories.Interfaces;

public interface IUserSessionRepository:IBaseRepository<UserSession>
{
    Task<UserSession> GetActiveSessionByUserIdAsync(Guid userId,CancellationToken cancellationToken=default);
    Task<IEnumerable<UserSession>> GetAllSessionsAsync(CancellationToken cancellationToken = default);
    void Update(UserSession userSession);
    Task<UserSession> GetSessionByRefreshTokenAsync(string refreshToken,CancellationToken cancellationToken=default);
    void DeleteSessionByUserId(Guid userId);

}