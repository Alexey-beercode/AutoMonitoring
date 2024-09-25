using AutoMonitoring.Domain.Entities.Implementations;

namespace AutoMonitoring.BLL.Factories.Implementations;

public class UserSessionFactory
{
    public UserSession Create(Guid userId, string deviceName, string refreshToken)
    {
        return new UserSession
        {
            UserId = userId,
            DeviceName = deviceName,
            RefreshToken = refreshToken,
            LastActive = DateTime.UtcNow,
            IsActive = true
        };
    }
}