namespace AutoMonitoring.BLL.DTOs.Implementations.Requests.User;

public class BlockUserDTO
{
    public DateTime? BlockUntil { get; set; }
    public Guid UserId { get; set; }
}