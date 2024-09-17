using System.Security.Claims;
using AutoMonitoring.Domain.Entities.Implementations;

namespace AutoMonitoring.BLL.Services.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    List<Claim> CreateClaims(User user,List<Role> roles);
}