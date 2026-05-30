using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.Loans.GetById;

public sealed record GetLoanByIdQuery(Guid Id) : IQuery<LoanDetailsResponse>;
