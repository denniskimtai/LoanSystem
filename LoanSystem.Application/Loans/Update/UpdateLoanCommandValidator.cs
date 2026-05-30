using FluentValidation;

namespace LoanSystem.Application.Loans.Update;

public sealed class UpdateLoanCommandValidator : AbstractValidator<UpdateLoanCommand>
{
    public UpdateLoanCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Loan ID is required.");

        RuleFor(x => x.Principal)
            .GreaterThan(0).WithMessage("Principal amount must be greater than zero.");

        RuleFor(x => x.InterestAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Interest amount must be non-negative.");

        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required.");

        RuleFor(x => x.LoId)
            .NotEmpty().WithMessage("Loan Officer ID is required.");

        RuleFor(x => x.CoId)
            .NotEmpty().WithMessage("Credit Officer ID is required.");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid loan type.");
    }
}
