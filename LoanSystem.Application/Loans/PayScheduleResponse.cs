using LoanSystem.Domain.Enums;

namespace LoanSystem.Application.Loans;

public sealed record PayScheduleResponse(
    Guid Id,
    DateOnly ScheduledDate,
    decimal Amount,
    decimal Balance,
    PaymentStatus Status);
