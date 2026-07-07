using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.LoanProducts.Update;

public sealed class UpdateLoanProductCommandHandler : ICommandHandler<UpdateLoanProductCommand>
{
    private readonly ILoanProductRepository _loanProductRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateLoanProductCommandHandler(ILoanProductRepository loanProductRepository, IUnitOfWork unitOfWork)
    {
        _loanProductRepository = loanProductRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateLoanProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _loanProductRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product is null)
        {
            return Result.Failure(new Error("LoanProduct.NotFound", "The specified loan product does not exist."));
        }

        var interestRate = request.InterestRate > 1 ? request.InterestRate / 100m : request.InterestRate;
        product.Update(
            request.Name,
            request.MinAmount,
            request.MaxAmount,
            interestRate,
            request.RepaymentDays);

        _loanProductRepository.Update(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
