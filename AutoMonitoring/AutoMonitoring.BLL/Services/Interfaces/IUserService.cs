using AutoMonitoring.BLL.DTOs.Implementations.Requests.User;
using AutoMonitoring.BLL.DTOs.Implementations.Responses.User;

namespace AutoMonitoring.BLL.Services.Interfaces;

public interface IUserService
{
    Task RegisterAsync(LoginDTO lOginDto,CancellationToken cancellationToken=default);
    Task<string> LoginAsync(LoginDTO lOginDto,CancellationToken cancellationToken=default);
    Task<IEnumerable<UserResponseDTO>> GetAllAsync(CancellationToken cancellationToken = default);
}