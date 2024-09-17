using AutoMonitoring.BLL.Services.Interfaces;
using AutoMonitoring.DAL.Infrastructure;
using AutoMonitoring.DAL.Repositories.Interfaces;

namespace AutoMonitoring.BLL.Services.Implementations;

public class UserSessionService:IUserSessionService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserSessionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<bool> IsSessionActiveAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task CreateOrUpdateSessionAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task InvalidateSessionAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}