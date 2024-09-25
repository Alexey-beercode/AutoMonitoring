using AutoMapper;
using AutoMonitoring.BLL.DTOs.Implementations.Requests.Token;
using AutoMonitoring.BLL.DTOs.Implementations.Requests.User;
using AutoMonitoring.BLL.DTOs.Implementations.Responses.Token;
using AutoMonitoring.BLL.DTOs.Implementations.Responses.User;
using AutoMonitoring.BLL.Exceptions;
using AutoMonitoring.BLL.Services.Interfaces;
using AutoMonitoring.DAL.Infrastructure;
using AutoMonitoring.DAL.Repositories.Interfaces;
using AutoMonitoring.Domain.Entities.Implementations;
using Microsoft.Extensions.Configuration;

namespace AutoMonitoring.BLL.Services.Implementations;

public class UserService:IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly IUserSessionService _userSessionService;
    private readonly IConfiguration _configuration;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService, IUserSessionService userSessionService, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _tokenService = tokenService;
        _userSessionService = userSessionService;
        _configuration = configuration;
    }

    public async Task RegisterAsync(LoginDTO loginDto, CancellationToken cancellationToken = default)
    {
        var userFromDb = await _unitOfWork.Users.GetByLoginAsync(loginDto.Login, cancellationToken);
        if (userFromDb==null)
        {
            throw new AlreadyExistsException("User");
        }
        var user = _mapper.Map<User>(loginDto);
        await _unitOfWork.Users.CreateAsync(user, cancellationToken);
    }

    public async Task<TokenDTO> LoginAsync(LoginDTO loginDto, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetByLoginAsync(loginDto.Login, cancellationToken);
        if (user == null || user.Password !=loginDto.Password) 
        {
            throw new UnauthorizedAccessException("Invalid login or password.");
        }
        
        var session = await _userSessionService.GetActiveSessionByUserIdAsync(user.Id, cancellationToken);

        string refreshToken;
        if (session != null)
        {
            if (session.DeviceName == loginDto.DeviceName)
            {
                refreshToken = session.RefreshToken;
                await _userSessionService.UpdateSessionActivityAsync(user.Id, cancellationToken);
            }
            else
            {
                // Если устройство другое — выбрасываем ошибку
                throw new UnauthorizedAccessException("Access denied. Session is active on another device.");
            }
        }
        else
        {
            // Если сессии нет, создаем новую и генерируем RefreshToken
            refreshToken = _tokenService.GenerateRefreshToken();
            await _userSessionService.CreateOrUpdateSessionAsync(user.Id, loginDto.DeviceName, refreshToken,DateTime.UtcNow.AddDays(_configuration.GetSection("Jwt:RefreshTokenExpirationDays").Get<int>()), cancellationToken);
        }

        // Шаг 3: Создаем токены и возвращаем их
        var claims = _tokenService.CreateClaims(user, await _unitOfWork.Roles.GetRolesByUserIdAsync(user.Id, cancellationToken));
        var accessToken = _tokenService.GenerateAccessToken(claims);

        // Возвращаем Access и Refresh токены
        return new TokenDTO(){AccessToken = accessToken,RefreshToken = refreshToken};
    }

    public async Task<IEnumerable<UserResponseDTO>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await _unitOfWork.Users.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<UserResponseDTO>>(users);
    }

    public async Task<TokenDTO> RefreshTokenAsync(RefreshTokenDTO refreshTokenDto, CancellationToken cancellationToken = default)
    {
        var session = await _userSessionService.GetSessionByRefreshTokenAsync(refreshTokenDto.RefreshToken, cancellationToken);
        if (session == null || !session.IsActive)
        {
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");
        }
        
        if (session.DeviceName != refreshTokenDto.DeviceName)
        {
            throw new UnauthorizedAccessException("Access denied. Refresh token is not valid for this device.");
        }
        
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        session.RefreshToken = newRefreshToken;
        session.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_configuration.GetSection("Jwt:RefreshTokenExpirationDays").Get<int>()); 
        session.LastActive = DateTime.UtcNow;
        _unitOfWork.UserSessions.Update(session);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        var user = await _unitOfWork.Users.GetByIdAsync(session.UserId, cancellationToken);
        var rolesByUser=  await _unitOfWork.Roles.GetRolesByUserIdAsync(user.Id, cancellationToken);
        var claims = _tokenService.CreateClaims(user,rolesByUser);
        var newAccessToken = _tokenService.GenerateAccessToken(claims);
        
        return new TokenDTO(){AccessToken = newAccessToken,RefreshToken = newRefreshToken}; 
    }
}