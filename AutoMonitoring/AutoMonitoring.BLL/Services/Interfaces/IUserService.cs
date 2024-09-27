using AutoMonitoring.BLL.DTOs.Implementations.Requests.Token;
using AutoMonitoring.BLL.DTOs.Implementations.Requests.User;
using AutoMonitoring.BLL.DTOs.Implementations.Responses.Token;
using AutoMonitoring.BLL.DTOs.Implementations.Responses.User;

namespace AutoMonitoring.BLL.Services.Interfaces;

public interface IUserService
{
    Task RegisterAsync(UserDTO userDto,CancellationToken cancellationToken=default);
    Task<TokenDTO> LoginAsync(LoginDTO loginDto,CancellationToken cancellationToken=default);
    Task<IEnumerable<UserResponseDTO>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TokenDTO> RefreshTokenAsync(RefreshTokenDTO refreshTokenDto, CancellationToken cancellationToken=default);
    Task BlockUserAsync(BlockUserDTO blockUserDto, CancellationToken cancellationToken = default);
}