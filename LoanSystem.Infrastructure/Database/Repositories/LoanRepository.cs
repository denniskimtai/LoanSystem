using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Entities.Loans;
using LoanSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LoanSystem.Infrastructure.Database.Repositories;

public sealed class LoanRepository : ILoanRepository
{
    private readonly AppDbContext _context;

    public LoanRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Loan?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Loans
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    public async Task<Loan?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Loans
            .Include(l => l.Customer)
            .Include(l => l.Product)
            .Include(l => l.Addons)
            .Include(l => l.Deductions)
            .Include(l => l.PaySchedules)
            .Include(l => l.Collaterals)
            .Include(l => l.Payments)
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    public void Add(Loan loan)
    {
        _context.Loans.Add(loan);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Loans.AnyAsync(l => l.Id == id, cancellationToken);
    }

    public async Task<string> GenerateLoanCodeAsync(CancellationToken cancellationToken = default)
    {
        var count = await _context.Loans.CountAsync(cancellationToken);
        return $"LN-{(count + 1).ToString().PadLeft(6, '0')}";
    }

    public async Task<(IReadOnlyCollection<Loan> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        string? searchTerm = null,
        LoanStatus? status = null,
        LoanStage? stage = null,
        Guid? customerId = null,
        Guid? branchId = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Loan> query = _context.Loans
            .Include(l => l.Customer)
            .Include(l => l.Product);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(l => 
                l.Code.Contains(searchTerm) || 
                l.Customer.FullName.Contains(searchTerm));
        }

        if (status.HasValue)
        {
            query = query.Where(l => l.Status == status.Value);
        }

        if (stage.HasValue)
        {
            query = query.Where(l => l.Stage == stage.Value);
        }

        if (customerId.HasValue)
        {
            query = query.Where(l => l.CustomerId == customerId.Value);
        }

        if (branchId.HasValue)
        {
            query = query.Where(l => l.BranchId == branchId.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(l => l.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
