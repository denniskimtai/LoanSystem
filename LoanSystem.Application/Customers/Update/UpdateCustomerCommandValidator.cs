using FluentValidation;

namespace LoanSystem.Application.Customers.Update;

public sealed class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(150).WithMessage("Full name must not exceed 150 characters.");

        RuleFor(x => x.NationalId)
            .NotEmpty().WithMessage("National ID is required.")
            .MaximumLength(50).WithMessage("National ID must not exceed 50 characters.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters.");

        RuleFor(x => x.PhotoUrl)
            .MaximumLength(500).WithMessage("Photo URL must not exceed 500 characters.");

        RuleFor(x => x.PhysicalAddress)
            .NotEmpty().WithMessage("Physical address is required.");

        RuleFor(x => x.HomeGeoLocation)
            .MaximumLength(200).WithMessage("Home GeoLocation must not exceed 200 characters.");

        RuleFor(x => x.Town)
            .NotEmpty().WithMessage("Town is required.")
            .MaximumLength(100).WithMessage("Town must not exceed 100 characters.");

        RuleFor(x => x.County)
            .NotEmpty().WithMessage("County is required.")
            .MaximumLength(100).WithMessage("County must not exceed 100 characters.");

        RuleFor(x => x.PostalAddress)
            .NotEmpty().WithMessage("Postal address is required.")
            .MaximumLength(200).WithMessage("Postal address must not exceed 200 characters.");
    }
}
