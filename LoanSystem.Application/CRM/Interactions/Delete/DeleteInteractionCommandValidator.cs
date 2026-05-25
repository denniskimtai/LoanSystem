using FluentValidation;

namespace LoanSystem.Application.CRM.Interactions.Delete;

public sealed class DeleteInteractionCommandValidator : AbstractValidator<DeleteInteractionCommand>
{
    public DeleteInteractionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Interaction ID is required.");
    }
}
