using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Entities.Identity;
using LoanSystem.Domain.Enums;
using LoanSystem.Domain.Primitives;
using Microsoft.AspNetCore.Identity;

namespace LoanSystem.Application.Loans.Update;

public sealed class UpdateLoanCommandHandler : ICommandHandler<UpdateLoanCommand>
{
    private readonly ILoanRepository _loanRepository;
    private readonly ILoanProductRepository _loanProductRepository;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateLoanCommandHandler(
        ILoanRepository loanRepository,
        ILoanProductRepository loanProductRepository,
        UserManager<User> userManager,
        IUnitOfWork unitOfWork)
    {
        _loanRepository = loanRepository;
        _loanProductRepository = loanProductRepository;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateLoanCommand request, CancellationToken cancellationToken)
    {
        // 1. Get Loan
        var loan = await _loanRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);
        if (loan is null)
        {
            return Result.Failure(new Error("Loan.NotFound", "The specified loan does not exist."));
        }

        // 2. Enforce update state lock (only allow updates if Created and Initiation)
        if (loan.Status != LoanStatus.Created || loan.Stage != LoanStage.Initiation)
        {
            return Result.Failure(new Error("Loan.UpdateBlocked", "Financial details can only be updated for loans in the Created status and Initiation stage."));
        }

        // 3. Verify Loan Product exists
        var product = await _loanProductRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
        {
            return Result.Failure(new Error("LoanProduct.NotFound", "The specified loan product does not exist."));
        }

        // 4. Verify Principal is within Product limits
        if (request.Principal < product.MinAmount || request.Principal > product.MaxAmount)
        {
            return Result.Failure(new Error("Loan.InvalidPrincipal", $"The principal amount must be between {product.MinAmount} and {product.MaxAmount} for this product."));
        }

        // 5. Verify Loan Officer exists
        var loanOfficer = await _userManager.FindByIdAsync(request.LoId.ToString());
        if (loanOfficer is null || !loanOfficer.IsActive)
        {
            return Result.Failure(new Error("Loan.InvalidLoanOfficer", "The specified Loan Officer does not exist or is inactive."));
        }

        // 6. Verify Credit Officer exists
        var creditOfficer = await _userManager.FindByIdAsync(request.CoId.ToString());
        if (creditOfficer is null || !creditOfficer.IsActive)
        {
            return Result.Failure(new Error("Loan.InvalidCreditOfficer", "The specified Credit Officer does not exist or is inactive."));
        }

        // 7. Update loan details
        loan.UpdateDetails(
            request.Principal,
            request.InterestAmount,
            request.ProductId,
            request.LoId,
            request.CoId,
            request.Type);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
