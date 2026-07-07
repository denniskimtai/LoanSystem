using FluentValidation;

namespace LoanSystem.Application.LoanProducts.Create;

public sealed class CreateLoanProductCommandValidator : AbstractValidator<CreateLoanProductCommand>
{
    public CreateLoanProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.MinAmount)
            .GreaterThanOrEqualTo(3000)
            .WithMessage("Minimum loan limit must be 3000 Ksh onwards.");

        RuleFor(x => x.MaxAmount)
            .GreaterThan(x => x.MinAmount)
            .WithMessage("MaxAmount must be greater than MinAmount.");

        RuleFor(x => x.InterestRate)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.RepaymentDays)
            .Must(x => x == 30 || x == 60 || x == 90)
            .WithMessage("Repayment days must be 30, 60, or 90 days.");
    }
}
