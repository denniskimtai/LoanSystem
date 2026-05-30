using FluentValidation;

namespace LoanSystem.Application.Customers.Create;

public sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
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
            .NotEmpty().WithMessage("Photo URL is required.")
            .MaximumLength(500).WithMessage("Photo URL must not exceed 500 characters.");

        RuleFor(x => x.PhysicalAddress)
            .NotEmpty().WithMessage("Physical address is required.");

        RuleFor(x => x.HomeGeoLocation)
            .NotEmpty().WithMessage("Home GeoLocation is required.")
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

        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("Branch ID is required.");

        RuleFor(x => x.CreatedById)
            .NotEmpty().WithMessage("Creator User ID is required.");

        // Conditional nested validation for BusinessInfo
        When(x => x.BusinessInfo != null, () =>
        {
            RuleFor(x => x.BusinessInfo!.BusinessName)
                .NotEmpty().WithMessage("Business name is required.")
                .MaximumLength(200).WithMessage("Business name must not exceed 200 characters.");

            RuleFor(x => x.BusinessInfo!.BusinessType)
                .NotEmpty().WithMessage("Business type is required.")
                .MaximumLength(100).WithMessage("Business type must not exceed 100 characters.");

            RuleFor(x => x.BusinessInfo!.BusinessDirection)
                .NotEmpty().WithMessage("Business direction is required.");

            RuleFor(x => x.BusinessInfo!.BusinessGeoLocation)
                .NotEmpty().WithMessage("Business GeoLocation is required.")
                .MaximumLength(200).WithMessage("Business GeoLocation must not exceed 200 characters.");

            RuleFor(x => x.BusinessInfo!.CurrentStockValue)
                .GreaterThanOrEqualTo(0).WithMessage("Current stock value must be non-negative.");

            RuleFor(x => x.BusinessInfo!.WeeklyGrossProfit)
                .GreaterThanOrEqualTo(0).WithMessage("Weekly gross profit must be non-negative.");

            RuleFor(x => x.BusinessInfo!.WeeklyNetProfit)
                .GreaterThanOrEqualTo(0).WithMessage("Weekly net profit must be non-negative.");

            RuleFor(x => x.BusinessInfo!.WeeklyExpenses)
                .GreaterThanOrEqualTo(0).WithMessage("Weekly expenses must be non-negative.");

            RuleFor(x => x.BusinessInfo!.YearsInBusiness)
                .GreaterThanOrEqualTo(0).WithMessage("Years in business must be non-negative.");

            RuleFor(x => x.BusinessInfo!.LeadType)
                .NotEmpty().WithMessage("Lead type is required.")
                .MaximumLength(50).WithMessage("Lead type must not exceed 50 characters.");

            RuleFor(x => x.BusinessInfo!.ProposedLimit)
                .GreaterThanOrEqualTo(0).WithMessage("Proposed limit must be non-negative.");
        });

        // Conditional nested validation for SecondaryInfo
        When(x => x.SecondaryInfo != null, () =>
        {
            RuleFor(x => x.SecondaryInfo!.MaritalStatus)
                .IsInEnum().WithMessage("Invalid marital status.");

            RuleFor(x => x.SecondaryInfo!.Dependents)
                .GreaterThanOrEqualTo(0).WithMessage("Dependents must be non-negative.");

            RuleFor(x => x.SecondaryInfo!.Estate)
                .NotEmpty().WithMessage("Estate is required.")
                .MaximumLength(100).WithMessage("Estate must not exceed 100 characters.");

            RuleFor(x => x.SecondaryInfo!.HouseNumber)
                .NotEmpty().WithMessage("House number is required.")
                .MaximumLength(50).WithMessage("House number must not exceed 50 characters.");

            RuleFor(x => x.SecondaryInfo!.Ownership)
                .IsInEnum().WithMessage("Invalid ownership status.");

            RuleFor(x => x.SecondaryInfo!.HomeAssetValue)
                .GreaterThanOrEqualTo(0).WithMessage("Home asset value must be non-negative.");

            RuleFor(x => x.SecondaryInfo!.NearestLandmark)
                .NotEmpty().WithMessage("Nearest landmark is required.")
                .MaximumLength(200).WithMessage("Nearest landmark must not exceed 200 characters.");

            RuleFor(x => x.SecondaryInfo!.GeoLocation)
                .NotEmpty().WithMessage("GeoLocation is required.")
                .MaximumLength(200).WithMessage("GeoLocation must not exceed 200 characters.");

            RuleFor(x => x.SecondaryInfo!.HeardVia)
                .NotEmpty().WithMessage("Heard via is required.")
                .MaximumLength(100).WithMessage("Heard via must not exceed 100 characters.");
        });

        // Conditional nested validation for Guarantors
        RuleForEach(x => x.Guarantors)
            .SetValidator(new CreateGuarantorInputValidator());

        // Conditional nested validation for Referees
        RuleForEach(x => x.Referees)
            .SetValidator(new CreateRefereeInputValidator());
    }
}

public sealed class CreateGuarantorInputValidator : AbstractValidator<CreateGuarantorInput>
{
    public CreateGuarantorInputValidator()
    {
        RuleFor(g => g.Name)
            .NotEmpty().WithMessage("Guarantor name is required.")
            .MaximumLength(150).WithMessage("Guarantor name must not exceed 150 characters.");

        RuleFor(g => g.IdNumber)
            .NotEmpty().WithMessage("Guarantor ID number is required.")
            .MaximumLength(50).WithMessage("Guarantor ID number must not exceed 50 characters.");

        RuleFor(g => g.Phone)
            .NotEmpty().WithMessage("Guarantor phone is required.")
            .MaximumLength(20).WithMessage("Guarantor phone must not exceed 20 characters.");

        RuleFor(g => g.AmountGuaranteed)
            .GreaterThan(0).WithMessage("Amount guaranteed must be greater than zero.");

        RuleFor(g => g.Relationship)
            .NotEmpty().WithMessage("Guarantor relationship is required.")
            .MaximumLength(100).WithMessage("Guarantor relationship must not exceed 100 characters.");
    }
}

public sealed class CreateRefereeInputValidator : AbstractValidator<CreateRefereeInput>
{
    public CreateRefereeInputValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty().WithMessage("Referee name is required.")
            .MaximumLength(150).WithMessage("Referee name must not exceed 150 characters.");

        RuleFor(r => r.Phone)
            .NotEmpty().WithMessage("Referee phone is required.")
            .MaximumLength(20).WithMessage("Referee phone must not exceed 20 characters.");

        RuleFor(r => r.PhysicalAddress)
            .NotEmpty().WithMessage("Referee physical address is required.");

        RuleFor(r => r.Relationship)
            .NotEmpty().WithMessage("Referee relationship is required.")
            .MaximumLength(100).WithMessage("Referee relationship must not exceed 100 characters.");
    }
}
