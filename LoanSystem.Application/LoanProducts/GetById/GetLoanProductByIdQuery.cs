using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Loans;

namespace LoanSystem.Application.LoanProducts.GetById;

public sealed record GetLoanProductByIdQuery(Guid Id) : IQuery<LoanProductResponse>;
