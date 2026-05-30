using LoanSystem.Application.Abstractions;
using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.Loans.GetPaged;

public sealed class GetLoansQueryHandler : IQueryHandler<GetLoansQuery, PagedResult<LoanResponse>>
{
    private readonly ILoanRepository _loanRepository;

    public GetLoansQueryHandler(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }

    public async Task<Result<PagedResult<LoanResponse>>> Handle(GetLoansQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _loanRepository.GetPagedAsync(
            request.Page,
            request.PageSize,
            request.SearchTerm,
            request.Status,
            request.Stage,
            request.CustomerId,
            request.BranchId,
            cancellationToken);

        var mappedItems = items.Select(l => new LoanResponse(
            l.Id,
            l.Code,
            l.CustomerId,
            l.Customer?.FullName ?? string.Empty,
            l.ProductId,
            l.Product?.Name ?? string.Empty,
            l.Principal,
            l.Balance,
            l.Type,
            l.Stage,
            l.Status,
            l.CreatedAt
        )).ToList();

        var pagedResult = new PagedResult<LoanResponse>(
            mappedItems,
            request.Page,
            request.PageSize,
            totalCount);

        return Result.Success(pagedResult);
    }
}
