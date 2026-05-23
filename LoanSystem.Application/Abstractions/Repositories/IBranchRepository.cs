using LoanSystem.Domain.Entities.Identity;

namespace LoanSystem.Application.Abstractions.Repositories;

public interface IBranchRepository
{
    Task<Branch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
