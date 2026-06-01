using FluentValidation;

namespace LoanSystem.Application.LoanProducts.Update;

public sealed class UpdateLoanProductCommandValidator : AbstractValidator<UpdateLoanProductCommand>
{
    public UpdateLoanProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.MinAmount)
            .GreaterThan(0);

        RuleFor(x => x.MaxAmount)
            .GreaterThan(x => x.MinAmount)
            .WithMessage("MaxAmount must be greater than MinAmount.");

        RuleFor(x => x.InterestRate)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.RepaymentDays)
            .GreaterThan(0);
    }
}
