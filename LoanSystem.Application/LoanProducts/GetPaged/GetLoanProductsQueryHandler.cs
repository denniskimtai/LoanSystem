using LoanSystem.Application.Abstractions;
using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Application.Loans;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.LoanProducts.GetPaged;

public sealed class GetLoanProductsQueryHandler : IQueryHandler<GetLoanProductsQuery, PagedResult<LoanProductResponse>>
{
    private readonly ILoanProductRepository _loanProductRepository;

    public GetLoanProductsQueryHandler(ILoanProductRepository loanProductRepository)
    {
        _loanProductRepository = loanProductRepository;
    }

    public async Task<Result<PagedResult<LoanProductResponse>>> Handle(GetLoanProductsQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _loanProductRepository.GetPagedAsync(
            request.Page,
            request.PageSize,
            request.SearchTerm,
            cancellationToken);

        var mappedItems = items.Select(p => new LoanProductResponse(
            p.Id,
            p.Name,
            p.MinAmount,
            p.MaxAmount,
            p.InterestRate,
            p.RepaymentDays
        )).ToList();

        var pagedResult = new PagedResult<LoanProductResponse>(
            mappedItems,
            request.Page,
            request.PageSize,
            totalCount);

        return Result.Success(pagedResult);
    }
}
