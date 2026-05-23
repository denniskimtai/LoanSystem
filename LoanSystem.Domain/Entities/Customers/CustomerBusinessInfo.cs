using LoanSystem.Domain.Primitives;

namespace LoanSystem.Domain.Entities.Customers;

public sealed class CustomerBusinessInfo : BaseEntity
{
    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; } = null!;
    public string BusinessName { get; private set; }
    public string BusinessType { get; private set; }
    public string BusinessDirection { get; private set; }
    public string BusinessGeoLocation { get; private set; }
    public decimal CurrentStockValue { get; private set; }
    public decimal WeeklyGrossProfit { get; private set; }
    public decimal WeeklyNetProfit { get; private set; }
    public decimal WeeklyExpenses { get; private set; }
    public int YearsInBusiness { get; private set; }
    public bool OffersCredit { get; private set; }
    public string LeadType { get; private set; }
    public decimal ProposedLimit { get; private set; }
    public bool WouldLend { get; private set; }

    public CustomerBusinessInfo(Guid customerId, string businessName, string businessType, string businessDirection, string businessGeoLocation, decimal currentStockValue, decimal weeklyGrossProfit, decimal weeklyNetProfit, decimal weeklyExpenses, int yearsInBusiness, bool offersCredit, string leadType, decimal proposedLimit, bool wouldLend)
    {
        CustomerId = customerId;
        BusinessName = businessName;
        BusinessType = businessType;
        BusinessDirection = businessDirection;
        BusinessGeoLocation = businessGeoLocation;
        CurrentStockValue = currentStockValue;
        WeeklyGrossProfit = weeklyGrossProfit;
        WeeklyNetProfit = weeklyNetProfit;
        WeeklyExpenses = weeklyExpenses;
        YearsInBusiness = yearsInBusiness;
        OffersCredit = offersCredit;
        LeadType = leadType;
        ProposedLimit = proposedLimit;
        WouldLend = wouldLend;
    }

    private CustomerBusinessInfo() { } // EF Core
}
