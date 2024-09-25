using AutoMonitoring.BLL.DTOs.Implementations.Requests.User;
using AutoMonitoring.Domain.Entities.Implementations;

namespace AutoMonitoring.BLL.Services.Interfaces;

public interface IUserSessionService
{
    Task<bool> IsSessionActiveAsync(Guid userId, CancellationToken cancellationToken = default);

    Task CreateOrUpdateSessionAsync(Guid userId, string deviceName, string refreshToken,
        CancellationToken cancellationToken = default);
    Task BlockUserAsync(BlockUserDTO blockUserDto, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserSession>> GetAllSessionsAsync(CancellationToken cancellationToken = default);
    Task<UserSession> GetActiveSessionByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task RevokeSessionAsync(Guid userId, CancellationToken cancellationToken = default);
}