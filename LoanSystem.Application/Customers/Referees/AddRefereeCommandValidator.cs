using FluentValidation;

namespace LoanSystem.Application.Customers.Referees;

public sealed class AddRefereeCommandValidator : AbstractValidator<AddRefereeCommand>
{
    public AddRefereeCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Referee name is required.")
            .MaximumLength(150).WithMessage("Referee name must not exceed 150 characters.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Referee phone number is required.")
            .MaximumLength(20).WithMessage("Referee phone number must not exceed 20 characters.");

        RuleFor(x => x.PhysicalAddress)
            .NotEmpty().WithMessage("Referee physical address is required.");

        RuleFor(x => x.Relationship)
            .NotEmpty().WithMessage("Referee relationship is required.")
            .MaximumLength(100).WithMessage("Referee relationship must not exceed 100 characters.");
    }
}
