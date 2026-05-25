using FluentValidation;

namespace LoanSystem.Application.Customers.GetPaged;

public sealed class GetCustomersQueryValidator : AbstractValidator<GetCustomersQuery>
{
    public GetCustomersQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("Page size must be greater than or equal to 1.")
            .LessThanOrEqualTo(100).WithMessage("Page size must not exceed 100.");
    }
}
