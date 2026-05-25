using LoanSystem.Domain.Entities.Customers;
using LoanSystem.Domain.Enums;

namespace LoanSystem.Application.Abstractions.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Customer?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Customer customer);
    Task<bool> ExistsByNationalIdAsync(string nationalId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByPhoneAsync(string phone, CancellationToken cancellationToken = default);
    Task<(IReadOnlyCollection<Customer> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        string? searchTerm = null,
        CustomerStatus? status = null,
        Guid? branchId = null,
        CancellationToken cancellationToken = default);
}
