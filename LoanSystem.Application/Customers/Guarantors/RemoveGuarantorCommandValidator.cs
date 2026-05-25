using FluentValidation;

namespace LoanSystem.Application.Customers.Guarantors;

public sealed class RemoveGuarantorCommandValidator : AbstractValidator<RemoveGuarantorCommand>
{
    public RemoveGuarantorCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(x => x.GuarantorId)
            .NotEmpty().WithMessage("Guarantor ID is required.");
    }
}
