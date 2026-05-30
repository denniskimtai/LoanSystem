using FluentValidation;

namespace LoanSystem.Application.Customers.Guarantors;

public sealed class AddGuarantorCommandValidator : AbstractValidator<AddGuarantorCommand>
{
    public AddGuarantorCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Guarantor name is required.")
            .MaximumLength(150).WithMessage("Guarantor name must not exceed 150 characters.");

        RuleFor(x => x.IdNumber)
            .NotEmpty().WithMessage("Guarantor ID number is required.")
            .MaximumLength(50).WithMessage("Guarantor ID number must not exceed 50 characters.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Guarantor phone is required.")
            .MaximumLength(20).WithMessage("Guarantor phone must not exceed 20 characters.");

        RuleFor(x => x.AmountGuaranteed)
            .GreaterThan(0).WithMessage("Amount guaranteed must be greater than zero.");

        RuleFor(x => x.Relationship)
            .NotEmpty().WithMessage("Guarantor relationship is required.")
            .MaximumLength(100).WithMessage("Guarantor relationship must not exceed 100 characters.");
    }
}
