using FluentValidation;

namespace LoanSystem.Application.Loans.Create;

public sealed class CreateLoanCommandValidator : AbstractValidator<CreateLoanCommand>
{
    public CreateLoanCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required.");

        RuleFor(x => x.LoId)
            .NotEmpty().WithMessage("Loan Officer ID is required.");

        RuleFor(x => x.CoId)
            .NotEmpty().WithMessage("Credit Officer ID is required.");

        RuleFor(x => x.CreatedById)
            .NotEmpty().WithMessage("Creator ID is required.");

        RuleFor(x => x.Principal)
            .GreaterThan(0).WithMessage("Principal amount must be greater than zero.");

        RuleFor(x => x.InterestAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Interest amount must be non-negative.");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid loan type.");

        // Rule for Addons
        RuleForEach(x => x.Addons).ChildRules(addon =>
        {
            addon.RuleFor(a => a.Name)
                .NotEmpty().WithMessage("Addon name is required.")
                .MaximumLength(100).WithMessage("Addon name must not exceed 100 characters.");

            addon.RuleFor(a => a.Amount)
                .GreaterThan(0).WithMessage("Addon amount must be greater than zero.");
        });

        // Rule for Deductions
        RuleForEach(x => x.Deductions).ChildRules(deduction =>
        {
            deduction.RuleFor(d => d.Name)
                .NotEmpty().WithMessage("Deduction name is required.")
                .MaximumLength(100).WithMessage("Deduction name must not exceed 100 characters.");

            deduction.RuleFor(d => d.Amount)
                .GreaterThan(0).WithMessage("Deduction amount must be greater than zero.");
        });
    }
}
