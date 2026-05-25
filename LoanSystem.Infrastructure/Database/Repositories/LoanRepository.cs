using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Entities.Loans;
using Microsoft.EntityFrameworkCore;

namespace LoanSystem.Infrastructure.Database.Repositories;

public sealed class LoanRepository : ILoanRepository
{
    private readonly AppDbContext _context;

    public LoanRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Loan>().AnyAsync(l => l.Id == id, cancellationToken);
    }
}
