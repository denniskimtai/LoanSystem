using LoanSystem.Application.Abstractions;
using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Domain.Enums;

namespace LoanSystem.Application.Customers.GetPaged;

public sealed record GetCustomersQuery(
    int Page,
    int PageSize,
    string? SearchTerm = null,
    CustomerStatus? Status = null,
    Guid? BranchId = null) : IQuery<PagedResult<CustomerResponse>>;
