using FluentValidation;

namespace LoanSystem.Application.Customers.UpdateBusinessInfo;

public sealed class UpdateCustomerBusinessInfoCommandValidator : AbstractValidator<UpdateCustomerBusinessInfoCommand>
{
    public UpdateCustomerBusinessInfoCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(x => x.BusinessName)
            .NotEmpty().WithMessage("Business name is required.")
            .MaximumLength(200).WithMessage("Business name must not exceed 200 characters.");

        RuleFor(x => x.BusinessType)
            .NotEmpty().WithMessage("Business type is required.")
            .MaximumLength(100).WithMessage("Business type must not exceed 100 characters.");

        RuleFor(x => x.BusinessDirection)
            .NotEmpty().WithMessage("Business direction is required.");

        RuleFor(x => x.BusinessGeoLocation)
            .NotEmpty().WithMessage("Business GeoLocation is required.")
            .MaximumLength(200).WithMessage("Business GeoLocation must not exceed 200 characters.");

        RuleFor(x => x.CurrentStockValue)
            .GreaterThanOrEqualTo(0).WithMessage("Current stock value must be non-negative.");

        RuleFor(x => x.WeeklyGrossProfit)
            .GreaterThanOrEqualTo(0).WithMessage("Weekly gross profit must be non-negative.");

        RuleFor(x => x.WeeklyNetProfit)
            .GreaterThanOrEqualTo(0).WithMessage("Weekly net profit must be non-negative.");

        RuleFor(x => x.WeeklyExpenses)
            .GreaterThanOrEqualTo(0).WithMessage("Weekly expenses must be non-negative.");

        RuleFor(x => x.YearsInBusiness)
            .GreaterThanOrEqualTo(0).WithMessage("Years in business must be non-negative.");

        RuleFor(x => x.LeadType)
            .NotEmpty().WithMessage("Lead type is required.")
            .MaximumLength(50).WithMessage("Lead type must not exceed 50 characters.");

        RuleFor(x => x.ProposedLimit)
            .GreaterThanOrEqualTo(0).WithMessage("Proposed limit must be non-negative.");
    }
}
