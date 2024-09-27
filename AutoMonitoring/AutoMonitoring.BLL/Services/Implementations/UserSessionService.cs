using AutoMonitoring.BLL.DTOs.Implementations.Requests.User;
using AutoMonitoring.BLL.Exceptions;
using AutoMonitoring.BLL.Factories.Interfaces;
using AutoMonitoring.BLL.Services.Interfaces;
using AutoMonitoring.DAL.Infrastructure;
using AutoMonitoring.Domain.Entities.Implementations;

namespace AutoMonitoring.BLL.Services.Implementations;

public class UserSessionService:IUserSessionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserSessionFactory _userSessionFactory;
    public UserSessionService(IUnitOfWork unitOfWork, IUserSessionFactory userSessionFactory)
    {
        _unitOfWork = unitOfWork;
        _userSessionFactory = userSessionFactory;
    }

    public async Task<bool> IsSessionActiveAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.UserSessions.GetActiveSessionByUserIdAsync(userId,cancellationToken)==null;
    }

    public async Task CreateOrUpdateSessionAsync(Guid userId,string deviceName,string refreshToken,DateTime refreshTokenExpireTime, CancellationToken cancellationToken = default)
    {
        var session = await _unitOfWork.UserSessions.GetActiveSessionByUserIdAsync(userId, cancellationToken);
        if (session != null)
        {
            session.LastActive = DateTime.UtcNow;
            session.DeviceName = deviceName;
            session.RefreshToken = refreshToken;
            _unitOfWork.UserSessions.Update(session);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

        }
        else
        {
            session = _userSessionFactory.Create(userId, deviceName, refreshToken,refreshTokenExpireTime);
            await _unitOfWork.UserSessions.CreateAsync(session, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

    public Task<IEnumerable<UserSession>> GetAllSessionsAsync(CancellationToken cancellationToken = default)
    {
        return _unitOfWork.UserSessions.GetAllSessionsAsync(cancellationToken);
    }

    public async Task<UserSession> GetActiveSessionByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.UserSessions.GetActiveSessionByUserIdAsync(userId, cancellationToken);
    }

    public async Task RevokeSessionAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var activeSession = await _unitOfWork.UserSessions.GetActiveSessionByUserIdAsync(userId, cancellationToken);
        
        if (activeSession != null)
        {
            activeSession.IsActive = false;
            activeSession.RefreshToken = string.Empty;
            activeSession.RefreshTokenExpiryTime=DateTime.MinValue;
            _unitOfWork.UserSessions.Update(activeSession);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<UserSession> GetSessionByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var userSession = await _unitOfWork.UserSessions.GetSessionByRefreshTokenAsync(refreshToken, cancellationToken);
        if (userSession==null)
        {
            throw new EntityNotFoundException("UserSession with refresh token not found");
        }

        return userSession;
    }
    public async Task UpdateSessionActivityAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var session = await _unitOfWork.UserSessions.GetActiveSessionByUserIdAsync(userId, cancellationToken);
        if (session != null)
        {
            session.LastActive = DateTime.UtcNow;
            _unitOfWork.UserSessions.Update(session);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}