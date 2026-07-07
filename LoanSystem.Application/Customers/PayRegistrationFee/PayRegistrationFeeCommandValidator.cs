using FluentValidation;

namespace LoanSystem.Application.Customers.PayRegistrationFee;

public sealed class PayRegistrationFeeCommandValidator : AbstractValidator<PayRegistrationFeeCommand>
{
    public PayRegistrationFeeCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(x => x.Amount)
            .Equal(500m).WithMessage("Registration fee must be exactly 500 Ksh.");

        RuleFor(x => x.TransactionCode)
            .NotEmpty().WithMessage("Transaction code is required.")
            .MaximumLength(100);

        RuleFor(x => x.MpesaRef)
            .NotEmpty().WithMessage("M-Pesa reference is required.")
            .MaximumLength(100);

        RuleFor(x => x.PayMethod)
            .IsInEnum().WithMessage("Valid payment method is required.");

        RuleFor(x => x.RecordedById)
            .NotEmpty().WithMessage("Recording user ID is required.");
    }
}
