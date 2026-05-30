namespace LoanSystem.Api.Customers;

public sealed record UpdateCustomerBusinessInfoRequest(
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
    bool WouldLend);
