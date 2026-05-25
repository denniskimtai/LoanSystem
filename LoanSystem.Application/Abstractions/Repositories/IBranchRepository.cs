using LoanSystem.Domain.Entities.Identity;

namespace LoanSystem.Application.Abstractions.Repositories;

public interface IBranchRepository
{
    Task<Branch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Branch branch);
    Task<(IReadOnlyCollection<Branch> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
}
