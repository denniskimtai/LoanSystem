using LoanSystem.Application.Abstractions;
using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.Customers.GetPaged;

public sealed class GetCustomersQueryHandler : IQueryHandler<GetCustomersQuery, PagedResult<CustomerResponse>>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomersQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Result<PagedResult<CustomerResponse>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _customerRepository.GetPagedAsync(
            request.Page,
            request.PageSize,
            request.SearchTerm,
            request.Status,
            request.BranchId,
            cancellationToken);

        var mappedItems = items.Select(c => new CustomerResponse(
            c.Id,
            c.FullName,
            c.NationalId,
            c.Phone,
            c.PhotoUrl,
            c.Status,
            c.PhysicalAddress,
            c.Town,
            c.County,
            c.CurrentLimit,
            c.BranchId,
            c.CreatedAt,
            c.UpdatedAt)).ToList();

        var pagedResult = new PagedResult<CustomerResponse>(
            mappedItems,
            request.Page,
            request.PageSize,
            totalCount);

        return Result.Success(pagedResult);
    }
}
