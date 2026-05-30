using FluentValidation;

namespace LoanSystem.Application.Loans.GetPaged;

public sealed class GetLoansQueryValidator : AbstractValidator<GetLoansQuery>
{
    public GetLoansQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("Page size must be greater than or equal to 1.")
            .LessThanOrEqualTo(100).WithMessage("Page size must not exceed 100.");
    }
}
