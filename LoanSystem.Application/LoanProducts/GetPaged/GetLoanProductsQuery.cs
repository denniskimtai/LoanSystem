using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Loans;
using LoanSystem.Application.Abstractions;

namespace LoanSystem.Application.LoanProducts.GetPaged;

public sealed record GetLoanProductsQuery(
    int Page,
    int PageSize,
    string? SearchTerm) : IQuery<PagedResult<LoanProductResponse>>;
