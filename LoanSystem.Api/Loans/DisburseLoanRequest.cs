namespace LoanSystem.Api.Loans;

public sealed record DisburseLoanRequest(
    string MpesaCode,
    DateTime DisbursedAt);
