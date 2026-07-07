using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Domain.Enums;

namespace LoanSystem.Application.Customers.PayRegistrationFee;

public sealed record PayRegistrationFeeCommand(
    Guid CustomerId,
    decimal Amount,
    string TransactionCode,
    string MpesaRef,
    PaymentMethod PayMethod,
    Guid RecordedById) : ICommand;
