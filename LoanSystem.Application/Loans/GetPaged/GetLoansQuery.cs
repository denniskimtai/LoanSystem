using LoanSystem.Application.Abstractions;
using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Domain.Enums;

namespace LoanSystem.Application.Loans.GetPaged;

public sealed record GetLoansQuery(
    int Page,
    int PageSize,
    string? SearchTerm = null,
    LoanStatus? Status = null,
    LoanStage? Stage = null,
    Guid? CustomerId = null,
    Guid? BranchId = null) : IQuery<PagedResult<LoanResponse>>;
