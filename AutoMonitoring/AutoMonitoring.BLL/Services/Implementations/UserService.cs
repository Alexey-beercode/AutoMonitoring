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
using AutoMonitoring.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace AutoMonitoring.BLL.Services.Implementations;

public class UserService:IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly IUserSessionService _userSessionService;
    private readonly IConfiguration _configuration;
    private readonly IRoleService _roleService;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService, IUserSessionService userSessionService, IConfiguration configuration, IRoleService roleService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _tokenService = tokenService;
        _userSessionService = userSessionService;
        _configuration = configuration;
        _roleService = roleService;
    }

    public async Task RegisterAsync(UserDTO userDto, CancellationToken cancellationToken = default)
    {
        var userFromDb = await _unitOfWork.Users.GetByLoginAsync(userDto.Login, cancellationToken);
        if (userFromDb!=null)
        {
            throw new AlreadyExistsException("User");
        }
        var user = _mapper.Map<User>(userDto);
        var baseRole = await _unitOfWork.Roles.GetByNameAsync("Resident");
        
        await _unitOfWork.Users.CreateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _roleService.SetRoleToUserAsync(new UserRoleModel() { RoleId = baseRole.Id, UserId = user.Id },
            cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<TokenDTO> LoginAsync(LoginDTO loginDto, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetByLoginAsync(loginDto.Login, cancellationToken);
        if (user == null || user.Password !=loginDto.Password) 
        {
            throw new UnauthorizedAccessException("Invalid login or password.");
        }
        
        if (user.IsBlocked && user.BlockedUntil > DateTime.UtcNow)
        {
            throw new UserBlockedException($"User is blocked until {user.BlockedUntil}.");
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
                throw new UnauthorizedAccessException("Access denied. Session is active on another device.");
            }
        }
        else
        {
            refreshToken = _tokenService.GenerateRefreshToken();
            await _userSessionService.CreateOrUpdateSessionAsync(user.Id, loginDto.DeviceName, refreshToken,DateTime.UtcNow.AddDays(_configuration.GetSection("Jwt:RefreshTokenExpirationDays").Get<int>()), cancellationToken);
        }
        
        var claims = _tokenService.CreateClaims(user, await _unitOfWork.Roles.GetRolesByUserIdAsync(user.Id, cancellationToken));
        var accessToken = _tokenService.GenerateAccessToken(claims);
        
        return new TokenDTO(){AccessToken = accessToken,RefreshToken = refreshToken,UserId = user.Id};
    }

    public async Task<IEnumerable<UserResponseDTO>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await _unitOfWork.Users.GetAllAsync(cancellationToken);
        var usersReponses = new List<UserResponseDTO>();

        foreach (var user in users)
        {
            var userResponse = _mapper.Map<UserResponseDTO>(user);
            var session = await _userSessionService.GetActiveSessionByUserIdAsync(user.Id, cancellationToken);

            if (session != null)
            {
                _mapper.Map(session, userResponse);
            }

            usersReponses.Add(userResponse);
        }

        return usersReponses;
    }


    public async Task<TokenDTO> RefreshTokenAsync(RefreshTokenDTO refreshTokenDto, CancellationToken cancellationToken = default)
    {
        var session = await _userSessionService.GetSessionByRefreshTokenAsync(refreshTokenDto.RefreshToken, cancellationToken);
        if (session == null || !session.IsActive)
        {
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");
        }
        
        var user = await _unitOfWork.Users.GetByIdAsync(session.UserId, cancellationToken);
        if (user.IsBlocked && user.BlockedUntil > DateTime.UtcNow)
        {
            throw new UserBlockedException($"User is blocked until {user.BlockedUntil}.");
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

        var rolesByUser = await _unitOfWork.Roles.GetRolesByUserIdAsync(user.Id, cancellationToken);
        var claims = _tokenService.CreateClaims(user, rolesByUser);
        var newAccessToken = _tokenService.GenerateAccessToken(claims);

        return new TokenDTO() { AccessToken = newAccessToken, RefreshToken = newRefreshToken,UserId = user.Id};
    }
    public async Task BlockUserAsync(BlockUserDTO blockUserDto, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(blockUserDto.UserId, cancellationToken);
    
        if (user == null)
        {
            throw new EntityNotFoundException("User",blockUserDto.UserId);
        }

        user.IsBlocked = true;
        user.BlockedUntil = blockUserDto.BlockUntil ?? DateTime.MaxValue;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string login, CancellationToken cancellationToken = default)
    {
        var userFromDb = await _unitOfWork.Users.GetByLoginAsync(login, cancellationToken);
        if (userFromDb==null)
        {
            throw new EntityNotFoundException($"User with login : {login} not found");
        }
        _unitOfWork.Users.Delete(userFromDb);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _userSessionService.DeleteSessionByUserId(userFromDb.Id);
    }

    public async Task UnblockUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id, cancellationToken);
    
        if (user == null)
        {
            throw new EntityNotFoundException("User",id);
        }

        user.IsBlocked = false;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}