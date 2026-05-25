using FluentValidation;

namespace LoanSystem.Application.Branches.Create;

public sealed class CreateBranchCommandValidator : AbstractValidator<CreateBranchCommand>
{
    public CreateBranchCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Branch name is required.")
            .MaximumLength(100).WithMessage("Branch name must not exceed 100 characters.");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Branch location is required.")
            .MaximumLength(200).WithMessage("Branch location must not exceed 200 characters.");
    }
}
