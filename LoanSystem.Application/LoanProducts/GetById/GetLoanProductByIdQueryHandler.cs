using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Application.Loans;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.LoanProducts.GetById;

public sealed class GetLoanProductByIdQueryHandler : IQueryHandler<GetLoanProductByIdQuery, LoanProductResponse>
{
    private readonly ILoanProductRepository _loanProductRepository;

    public GetLoanProductByIdQueryHandler(ILoanProductRepository loanProductRepository)
    {
        _loanProductRepository = loanProductRepository;
    }

    public async Task<Result<LoanProductResponse>> Handle(GetLoanProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _loanProductRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product is null)
        {
            return Result.Failure<LoanProductResponse>(new Error("LoanProduct.NotFound", "The specified loan product does not exist."));
        }

        var response = new LoanProductResponse(
            product.Id,
            product.Name,
            product.MinAmount,
            product.MaxAmount,
            product.InterestRate,
            product.RepaymentDays);

        return Result.Success(response);
    }
}
