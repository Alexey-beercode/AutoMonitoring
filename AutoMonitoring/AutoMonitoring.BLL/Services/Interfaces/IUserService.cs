using EventMaster.BLL.DTOs.Implementations.Requests.User;
using EventMaster.BLL.DTOs.Responses.User;

namespace AutoMonitoring.BLL.Services.Interfaces;

public interface IUserService
{
    Task RegisterAsync(UserDTO userDto,CancellationToken cancellationToken=default);
    Task<string> LoginAsync(UserDTO userDto,CancellationToken cancellationToken=default);
    Task<IEnumerable<UserResponseDTO>> GetAllAsync(CancellationToken cancellationToken = default);
}