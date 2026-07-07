using LoanSystem.Domain.Enums;

namespace LoanSystem.Api.Customers;

public sealed record PayRegistrationFeeRequest(
    decimal Amount,
    string TransactionCode,
    string MpesaRef,
    PaymentMethod PayMethod);
