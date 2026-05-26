using LoanSystem.Domain.Enums;

namespace LoanSystem.Application.Loans;

public sealed record LoanResponse(
    Guid Id,
    string Code,
    Guid CustomerId,
    string CustomerName,
    Guid ProductId,
    string ProductName,
    decimal Principal,
    decimal Balance,
    LoanType Type,
    LoanStage Stage,
    LoanStatus Status,
    DateTime CreatedAt);
