using FluentValidation;

namespace LoanSystem.Application.Loans.Approve;

public sealed class ApproveLoanCommandValidator : AbstractValidator<ApproveLoanCommand>
{
    public ApproveLoanCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Loan ID is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");
    }
}
