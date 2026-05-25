namespace LoanSystem.Application.Customers;

public sealed record CustomerBusinessInfoResponse(
    Guid Id,
    string BusinessName,
    string BusinessType,
    string BusinessDirection,
    string BusinessGeoLocation,
    decimal CurrentStockValue,
    decimal WeeklyGrossProfit,
    decimal WeeklyNetProfit,
    decimal WeeklyExpenses,
    int YearsInBusiness,
    bool OffersCredit,
    string LeadType,
    decimal ProposedLimit,
    bool WouldLend,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
