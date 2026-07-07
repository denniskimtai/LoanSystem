using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Entities.Loans;
using LoanSystem.Domain.Entities.Identity;
using LoanSystem.Domain.Primitives;
using Microsoft.AspNetCore.Identity;

namespace LoanSystem.Application.Loans.Create;

public sealed class CreateLoanCommandHandler : ICommandHandler<CreateLoanCommand, Guid>
{
    private readonly ILoanRepository _loanRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ILoanProductRepository _loanProductRepository;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public CreateLoanCommandHandler(
        ILoanRepository loanRepository,
        ICustomerRepository customerRepository,
        ILoanProductRepository loanProductRepository,
        UserManager<User> userManager,
        IUnitOfWork unitOfWork)
    {
        _loanRepository = loanRepository;
        _customerRepository = customerRepository;
        _loanProductRepository = loanProductRepository;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
    {
        // 1. Verify Customer exists
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer is null)
        {
            return Result.Failure<Guid>(new Error("Customer.NotFound", "The specified customer does not exist."));
        }

        // 2. Verify Loan Product exists
        var product = await _loanProductRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
        {
            return Result.Failure<Guid>(new Error("LoanProduct.NotFound", "The specified loan product does not exist."));
        }

        // 3. Verify Principal is within Product limits
        if (request.Principal < product.MinAmount || request.Principal > product.MaxAmount)
        {
            return Result.Failure<Guid>(new Error("Loan.InvalidPrincipal", $"The principal amount must be between {product.MinAmount} and {product.MaxAmount} for this product."));
        }

        // 4. Verify Loan Officer exists
        var loanOfficer = await _userManager.FindByIdAsync(request.LoId.ToString());
        if (loanOfficer is null || !loanOfficer.IsActive)
        {
            return Result.Failure<Guid>(new Error("Loan.InvalidLoanOfficer", "The specified Loan Officer does not exist or is inactive."));
        }

        // 5. Verify Credit Officer exists
        var creditOfficer = await _userManager.FindByIdAsync(request.CoId.ToString());
        if (creditOfficer is null || !creditOfficer.IsActive)
        {
            return Result.Failure<Guid>(new Error("Loan.InvalidCreditOfficer", "The specified Credit Officer does not exist or is inactive."));
        }

        // 6. Generate Loan Code
        var loanCode = await _loanRepository.GenerateLoanCodeAsync(cancellationToken);

        // 7. Create Loan
        var interestAmount = Math.Round(request.Principal * (product.InterestRate / 100m), 2);
        var loan = new Loan(
            loanCode,
            request.CustomerId,
            request.ProductId,
            customer.BranchId,
            request.LoId,
            request.CoId,
            request.CreatedById,
            request.Principal,
            interestAmount,
            request.Type);

        // 8. Add Addons if provided
        if (request.Addons is not null)
        {
            foreach (var addonInput in request.Addons)
            {
                var addon = new LoanAddon(loan.Id, addonInput.Name, addonInput.Amount);
                loan.AddAddon(addon);
            }
        }

        // 9. Add Deductions if provided
        if (request.Deductions is not null)
        {
            foreach (var deductionInput in request.Deductions)
            {
                var deduction = new LoanDeduction(loan.Id, deductionInput.Name, deductionInput.Amount);
                loan.AddDeduction(deduction);
            }
        }

        // 10. Add to repo & Save
        _loanRepository.Add(loan);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(loan.Id);
    }
}
