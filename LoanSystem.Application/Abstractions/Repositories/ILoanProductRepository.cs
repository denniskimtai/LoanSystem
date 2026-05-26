using LoanSystem.Domain.Entities.Loans;

namespace LoanSystem.Application.Abstractions.Repositories;

public interface ILoanProductRepository
{
    Task<LoanProduct?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
