using LoanSystem.Domain.Entities.Loans;
using LoanSystem.Domain.Enums;

namespace LoanSystem.Application.Abstractions.Repositories;

public interface ILoanRepository
{
    Task<Loan?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Loan?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Loan loan);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<string> GenerateLoanCodeAsync(CancellationToken cancellationToken = default);
    Task<(IReadOnlyCollection<Loan> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        string? searchTerm = null,
        LoanStatus? status = null,
        LoanStage? stage = null,
        Guid? customerId = null,
        Guid? branchId = null,
        CancellationToken cancellationToken = default);

    Task<bool> HasAnyLoansAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<bool> HasAnyOtherLoansAsync(Guid customerId, Guid currentLoanId, CancellationToken cancellationToken = default);
}
