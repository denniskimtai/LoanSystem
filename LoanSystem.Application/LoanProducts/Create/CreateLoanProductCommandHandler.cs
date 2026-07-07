using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Entities.Loans;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.LoanProducts.Create;

public sealed class CreateLoanProductCommandHandler : ICommandHandler<CreateLoanProductCommand, Guid>
{
    private readonly ILoanProductRepository _loanProductRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateLoanProductCommandHandler(ILoanProductRepository loanProductRepository, IUnitOfWork unitOfWork)
    {
        _loanProductRepository = loanProductRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateLoanProductCommand request, CancellationToken cancellationToken)
    {
        var interestRate = request.InterestRate > 1 ? request.InterestRate / 100m : request.InterestRate;
        var product = new LoanProduct(
            request.Name,
            request.MinAmount,
            request.MaxAmount,
            interestRate,
            request.RepaymentDays);

        _loanProductRepository.Add(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(product.Id);
    }
}
