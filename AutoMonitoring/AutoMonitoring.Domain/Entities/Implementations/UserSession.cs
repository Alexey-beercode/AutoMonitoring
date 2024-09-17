using AutoMonitoring.Domain.Entities.Interfaces;

namespace AutoMonitoring.Domain.Entities.Implementations;

public class UserSession : BaseEntity
{
    public Guid UserId { get; set; }
    public DateTime LastActive { get; set; }
    public bool IsActive { get; set; }
    public string DeviceName { get; set; } 
    public string RefreshToken { get; set; } 
    public DateTime RefreshTokenExpiryTime { get; set; } 
}
