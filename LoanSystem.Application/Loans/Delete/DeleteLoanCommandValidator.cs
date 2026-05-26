using FluentValidation;

namespace LoanSystem.Application.Loans.Delete;

public sealed class DeleteLoanCommandValidator : AbstractValidator<DeleteLoanCommand>
{
    public DeleteLoanCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Loan ID is required.");
    }
}
