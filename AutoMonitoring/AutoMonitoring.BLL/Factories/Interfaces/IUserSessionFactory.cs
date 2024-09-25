using AutoMonitoring.Domain.Entities.Implementations;

namespace AutoMonitoring.BLL.Factories.Interfaces;

public interface IUserSessionFactory
{
    UserSession Create(Guid userId, string deviceName, string refreshToken);
}
