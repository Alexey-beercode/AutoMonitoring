namespace AutoMonitoring.BLL.DTOs.Implementations.Responses.Token;

public class TokenDTO
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public Guid UserId { get; set; }
}