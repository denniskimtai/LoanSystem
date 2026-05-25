namespace LoanSystem.Application.Abstractions.Repositories;

public interface ILoanRepository
{
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
