using LoanSystem.Domain.Entities.Customers;
using LoanSystem.Domain.Entities.Identity;
using LoanSystem.Domain.Entities.Loans;
using LoanSystem.Domain.Enums;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Domain.Entities.CRM;

public sealed class Interaction : BaseEntity
{
    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; } = null!;
    public Guid? LoanId { get; private set; }
    public Loan? Loan { get; private set; }
    public Guid AgentId { get; private set; }
    public User Agent { get; private set; } = null!;
    public string Mode { get; private set; }
    public string Purpose { get; private set; }
    public string OutcomeDetails { get; private set; }
    public string OutcomeStatus { get; private set; }
    public string Tag { get; private set; }
    public decimal? PromisedAmount { get; private set; }
    public string DefaultReason { get; private set; }
    public string NextSteps { get; private set; }
    public string LocationGeo { get; private set; }
    public DateOnly? NextInteractionDate { get; private set; }
    public DateTime InteractionAt { get; private set; }

    public Interaction(Guid customerId, Guid agentId, string mode, string purpose, string outcomeDetails, string outcomeStatus, string tag, string defaultReason, string nextSteps, string locationGeo, DateTime interactionAt)
    {
        CustomerId = customerId;
        AgentId = agentId;
        Mode = mode;
        Purpose = purpose;
        OutcomeDetails = outcomeDetails;
        OutcomeStatus = outcomeStatus;
        Tag = tag;
        DefaultReason = defaultReason;
        NextSteps = nextSteps;
        LocationGeo = locationGeo;
        InteractionAt = interactionAt;
    }

    private Interaction() { } // EF Core

    public void Update(
        string mode,
        string purpose,
        string outcomeDetails,
        string outcomeStatus,
        string tag,
        string defaultReason,
        string nextSteps,
        string locationGeo,
        DateTime interactionAt)
    {
        Mode = mode;
        Purpose = purpose;
        OutcomeDetails = outcomeDetails;
        OutcomeStatus = outcomeStatus;
        Tag = tag;
        DefaultReason = defaultReason;
        NextSteps = nextSteps;
        LocationGeo = locationGeo;
        InteractionAt = interactionAt;
        UpdateTimestamp();
    }

    public void SetLoan(Guid? loanId)
    {
        LoanId = loanId;
        UpdateTimestamp();
    }

    public void SetPromisedAmount(decimal? amount)
    {
        PromisedAmount = amount;
        UpdateTimestamp();
    }

    public void SetNextInteractionDate(DateOnly? date)
    {
        NextInteractionDate = date;
        UpdateTimestamp();
    }
}
