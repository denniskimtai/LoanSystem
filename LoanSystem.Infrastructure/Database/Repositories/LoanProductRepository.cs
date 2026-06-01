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

    public async Task<(IReadOnlyCollection<LoanProduct> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        string? searchTerm,
        CancellationToken cancellationToken = default)
    {
        IQueryable<LoanProduct> query = _context.LoanProducts;

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(p => EF.Functions.Like(p.Name, $"%{searchTerm}%"));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public void Add(LoanProduct product)
    {
        _context.LoanProducts.Add(product);
    }

    public void Update(LoanProduct product)
    {
        _context.LoanProducts.Update(product);
    }

    public void Delete(LoanProduct product)
    {
        _context.LoanProducts.Remove(product);
    }
}
