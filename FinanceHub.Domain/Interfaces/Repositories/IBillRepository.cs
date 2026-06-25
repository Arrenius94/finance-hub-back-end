using FinanceHub.Domain.Entities;

namespace FinanceHub.Domain.Interfaces.Repositories;

public interface IBillRepository
{
    Task <int> SaveAsync (Bill bill);
    void Added (Bill bill);
}