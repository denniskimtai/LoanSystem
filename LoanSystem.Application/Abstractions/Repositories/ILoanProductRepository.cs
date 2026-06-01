using LoanSystem.Domain.Entities.Loans;

namespace LoanSystem.Application.Abstractions.Repositories;

public interface ILoanProductRepository
{
    Task<LoanProduct?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<(IReadOnlyCollection<LoanProduct> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        string? searchTerm,
        CancellationToken cancellationToken = default);
    void Add(LoanProduct product);
    void Update(LoanProduct product);
    void Delete(LoanProduct product);
}
