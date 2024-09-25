using AutoMonitoring.Domain.Entities.Implementations;

namespace AutoMonitoring.DAL.Repositories.Interfaces;

public interface IUserRepository:IBaseRepository<User>
{
    Task<User> GetByLoginAsync(string login, CancellationToken cancellationToken=default);
    void Update(User user);
}