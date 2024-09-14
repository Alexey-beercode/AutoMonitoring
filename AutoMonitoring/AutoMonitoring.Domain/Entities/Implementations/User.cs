using AutoMonitoring.Domain.Entities.Interfaces;

namespace AutoMonitoring.Domain.Entities.Implementations;

public class User:BaseEntity
{
    public string Login { get; set; }
    public string PasswordHash { get; set; }
}