using FluentValidation;

namespace LoanSystem.Application.Loans.Reject;

public sealed class RejectLoanCommandValidator : AbstractValidator<RejectLoanCommand>
{
    public RejectLoanCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Loan ID is required.");
    }
}
