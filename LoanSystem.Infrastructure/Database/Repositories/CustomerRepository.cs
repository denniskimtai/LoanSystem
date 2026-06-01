using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Entities.Customers;
using LoanSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LoanSystem.Infrastructure.Database.Repositories;

public sealed class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Customer?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .Include(c => c.BusinessInfo)
            .Include(c => c.SecondaryInfo)
            .Include(c => c.Guarantors)
            .Include(c => c.Referees)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public void Add(Customer customer)
    {
        _context.Customers.Add(customer);
    }

    public void AddGuarantor(Guarantor guarantor)
    {
        _context.Guarantors.Add(guarantor);
    }

    public void AddReferee(Referee referee)
    {
        _context.Referees.Add(referee);
    }

    public async Task<bool> ExistsByNationalIdAsync(string nationalId, CancellationToken cancellationToken = default)
    {
        return await _context.Customers.AnyAsync(c => c.NationalId == nationalId, cancellationToken);
    }

    public async Task<bool> ExistsByPhoneAsync(string phone, CancellationToken cancellationToken = default)
    {
        return await _context.Customers.AnyAsync(c => c.Phone == phone, cancellationToken);
    }

    public async Task<(IReadOnlyCollection<Customer> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        string? searchTerm = null,
        CustomerStatus? status = null,
        Guid? branchId = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Customer> query = _context.Customers;

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(c => 
                c.FullName.Contains(searchTerm) || 
                c.NationalId.Contains(searchTerm) || 
                c.Phone.Contains(searchTerm));
        }

        if (status.HasValue)
        {
            query = query.Where(c => c.Status == status.Value);
        }

        if (branchId.HasValue)
        {
            query = query.Where(c => c.BranchId == branchId.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(c => c.FullName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
