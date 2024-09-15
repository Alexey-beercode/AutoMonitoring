namespace AutoMonitoring.BLL.Services.Interfaces;

public interface IUserSessionService
{
    Task<bool> IsSessionActiveAsync(Guid userId, CancellationToken cancellationToken = default);
    Task CreateOrUpdateSessionAsync(Guid userId,CancellationToken cancellationToken = default);
    Task InvalidateSessionAsync(Guid userId,CancellationToken cancellationToken=default);
}