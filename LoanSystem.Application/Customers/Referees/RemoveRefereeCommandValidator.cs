using FluentValidation;

namespace LoanSystem.Application.Customers.Referees;

public sealed class RemoveRefereeCommandValidator : AbstractValidator<RemoveRefereeCommand>
{
    public RemoveRefereeCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(x => x.RefereeId)
            .NotEmpty().WithMessage("Referee ID is required.");
    }
}
