using FluentValidation;
using LoanSystem.Domain.Enums;

namespace LoanSystem.Application.Customers.UpdateSecondaryInfo;

public sealed class UpdateCustomerSecondaryInfoCommandValidator : AbstractValidator<UpdateCustomerSecondaryInfoCommand>
{
    public UpdateCustomerSecondaryInfoCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(x => x.MaritalStatus)
            .IsInEnum().WithMessage("Invalid marital status.");

        RuleFor(x => x.Dependents)
            .GreaterThanOrEqualTo(0).WithMessage("Dependents must be non-negative.");

        RuleFor(x => x.Estate)
            .NotEmpty().WithMessage("Estate is required.")
            .MaximumLength(100).WithMessage("Estate must not exceed 100 characters.");

        RuleFor(x => x.HouseNumber)
            .NotEmpty().WithMessage("House number is required.")
            .MaximumLength(50).WithMessage("House number must not exceed 50 characters.");

        RuleFor(x => x.Ownership)
            .IsInEnum().WithMessage("Invalid ownership status.");

        // Rule for RentAmount when rented
        When(x => x.Ownership == HomeOwnership.Rented, () =>
        {
            RuleFor(x => x.RentAmount)
                .NotNull().WithMessage("Rent amount is required when home is rented.")
                .GreaterThanOrEqualTo(0).WithMessage("Rent amount must be non-negative.");
        });

        RuleFor(x => x.HomeAssetValue)
            .GreaterThanOrEqualTo(0).WithMessage("Home asset value must be non-negative.");

        RuleFor(x => x.NearestLandmark)
            .NotEmpty().WithMessage("Nearest landmark is required.")
            .MaximumLength(200).WithMessage("Nearest landmark must not exceed 200 characters.");

        RuleFor(x => x.GeoLocation)
            .NotEmpty().WithMessage("GeoLocation is required.")
            .MaximumLength(200).WithMessage("GeoLocation must not exceed 200 characters.");

        RuleFor(x => x.HeardVia)
            .NotEmpty().WithMessage("Heard via is required.")
            .MaximumLength(100).WithMessage("Heard via must not exceed 100 characters.");
    }
}
