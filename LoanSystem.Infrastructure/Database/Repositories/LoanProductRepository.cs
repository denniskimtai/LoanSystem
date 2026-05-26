using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Entities.Loans;
using Microsoft.EntityFrameworkCore;

namespace LoanSystem.Infrastructure.Database.Repositories;

public sealed class LoanProductRepository : ILoanProductRepository
{
    private readonly AppDbContext _context;

    public LoanProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<LoanProduct?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.LoanProducts
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}
