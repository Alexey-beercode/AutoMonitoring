namespace AutoMonitoring.BLL.DTOs.Implementations.Responses.User;

public class UserResponseDTO
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string DeviceName { get; set; }
    public DateTime LastActive { get; set; }
    public bool IsBlocked { get; set; }
    public DateTime BlockedUntil { get; set; }
    public bool IsActive { get; set; }
    public Guid Id { get; set; }
}