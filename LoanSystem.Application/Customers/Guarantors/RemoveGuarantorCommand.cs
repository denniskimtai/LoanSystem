using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.Customers.Guarantors;

public sealed record RemoveGuarantorCommand(
    Guid CustomerId,
    Guid GuarantorId) : ICommand;
