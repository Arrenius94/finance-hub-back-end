namespace FinanceHub.Domain.Entities;

public abstract class BaseEntity
{
    protected BaseEntity()
    {
        CreatedAt = DateTime.UtcNow;
    }
    public int Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
}