using AutoMonitoring.BLL.DTOs.Implementations.Requests.User;
using AutoMonitoring.BLL.DTOs.Implementations.Responses.User;
using AutoMonitoring.BLL.Services.Interfaces;
using AutoMonitoring.DAL.Infrastructure;
using AutoMonitoring.DAL.Repositories.Interfaces;

namespace AutoMonitoring.BLL.Services.Implementations;

public class UserService:IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task RegisterAsync(LoginDTO lOginDto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<string> LoginAsync(LoginDTO lOginDto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UserResponseDTO>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}