using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.Loans.Disburse;

public sealed record DisburseLoanCommand(
    Guid Id,
    string MpesaCode,
    DateTime DisbursedAt) : ICommand;
