using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.Customers.UpdateBusinessInfo;

public sealed record UpdateCustomerBusinessInfoCommand(
    Guid CustomerId,
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
    bool WouldLend) : ICommand;
