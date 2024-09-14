namespace AutoMonitoring.Domain.Entities.Interfaces;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
}