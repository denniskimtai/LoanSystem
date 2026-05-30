using FluentValidation;

namespace LoanSystem.Application.Loans.Disburse;

public sealed class DisburseLoanCommandValidator : AbstractValidator<DisburseLoanCommand>
{
    public DisburseLoanCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Loan ID is required.");

        RuleFor(x => x.MpesaCode)
            .NotEmpty().WithMessage("Mpesa Code is required.")
            .MaximumLength(50).WithMessage("Mpesa Code must not exceed 50 characters.");

        RuleFor(x => x.DisbursedAt)
            .NotEmpty().WithMessage("Disbursed Date is required.");
    }
}
