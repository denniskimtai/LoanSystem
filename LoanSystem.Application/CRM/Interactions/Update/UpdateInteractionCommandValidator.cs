using FluentValidation;
using LoanSystem.Domain.Enums;

namespace LoanSystem.Application.CRM.Interactions.Update;

public sealed class UpdateInteractionCommandValidator : AbstractValidator<UpdateInteractionCommand>
{
    public UpdateInteractionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Interaction ID is required.");

        RuleFor(x => x.Mode)
            .NotEmpty().WithMessage("Interaction mode is required.")
            .MaximumLength(50).WithMessage("Interaction mode must not exceed 50 characters.");

        RuleFor(x => x.Purpose)
            .NotEmpty().WithMessage("Purpose is required.")
            .MaximumLength(200).WithMessage("Purpose must not exceed 200 characters.");

        RuleFor(x => x.OutcomeDetails)
            .NotEmpty().WithMessage("Outcome details are required.");

        RuleFor(x => x.OutcomeStatus)
            .NotEmpty().WithMessage("Outcome status is required.")
            .MaximumLength(50).WithMessage("Outcome status must not exceed 50 characters.");

        RuleFor(x => x.Tag)
            .NotEmpty().WithMessage("Interaction tag is required.")
            .MaximumLength(50).WithMessage("Interaction tag must not exceed 50 characters.");

        // Conditional validation for PromisedAmount
        When(x => !string.IsNullOrEmpty(x.OutcomeStatus) && x.OutcomeStatus.Equals("PromisedToPay", StringComparison.OrdinalIgnoreCase), () =>
        {
            RuleFor(x => x.PromisedAmount)
                .NotNull().WithMessage("Promised amount is required when outcome is Promised To Pay.")
                .GreaterThan(0).WithMessage("Promised amount must be greater than zero.");
        });

        RuleFor(x => x.DefaultReason)
            .MaximumLength(300).WithMessage("Default reason must not exceed 300 characters.");

        RuleFor(x => x.NextSteps)
            .NotEmpty().WithMessage("Next steps are required.");

        RuleFor(x => x.LocationGeo)
            .NotEmpty().WithMessage("Location GeoCoordinates are required.")
            .MaximumLength(200).WithMessage("Location GeoCoordinates must not exceed 200 characters.");

        RuleFor(x => x.InteractionAt)
            .NotEmpty().WithMessage("Interaction timestamp is required.");

        // Future date check for next interaction
        RuleFor(x => x.NextInteractionDate)
            .Must(date => date == null || date >= DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Next interaction date must be today or in the future.");
    }
}
