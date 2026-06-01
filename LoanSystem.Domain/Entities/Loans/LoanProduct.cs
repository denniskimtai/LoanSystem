using LoanSystem.Domain.Primitives;

namespace LoanSystem.Domain.Entities.Loans;

public sealed class LoanProduct : BaseEntity
{
    public string Name { get; private set; }
    public decimal MinAmount { get; private set; }
    public decimal MaxAmount { get; private set; }
    public decimal InterestRate { get; private set; }
    public int RepaymentDays { get; private set; }

    public LoanProduct(string name, decimal minAmount, decimal maxAmount, decimal interestRate, int repaymentDays)
    {
        Name = name;
        MinAmount = minAmount;
        MaxAmount = maxAmount;
        InterestRate = interestRate;
        RepaymentDays = repaymentDays;
    }

    public void Update(string name, decimal minAmount, decimal maxAmount, decimal interestRate, int repaymentDays)
    {
        Name = name;
        MinAmount = minAmount;
        MaxAmount = maxAmount;
        InterestRate = interestRate;
        RepaymentDays = repaymentDays;
    }

    private LoanProduct() { } // EF Core
}
