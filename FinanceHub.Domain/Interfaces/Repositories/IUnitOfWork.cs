namespace FinanceHub.Domain.Interfaces.Repositories;

public interface IUnitOfWork
{
    Task CommitAsync();
}