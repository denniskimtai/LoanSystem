using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Entities.CRM;
using LoanSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LoanSystem.Infrastructure.Database.Repositories;

public sealed class InteractionRepository : IInteractionRepository
{
    private readonly AppDbContext _context;

    public InteractionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Interaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Interactions
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public void Add(Interaction interaction)
    {
        _context.Interactions.Add(interaction);
    }

    public async Task<(IReadOnlyCollection<Interaction> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        Guid? customerId = null,
        Guid? agentId = null,
        string? tag = null,
        string? outcomeStatus = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Interaction> query = _context.Interactions;

        if (customerId.HasValue)
        {
            query = query.Where(i => i.CustomerId == customerId.Value);
        }

        if (agentId.HasValue)
        {
            query = query.Where(i => i.AgentId == agentId.Value);
        }

        if (!string.IsNullOrEmpty(tag))
        {
            query = query.Where(i => i.Tag == tag);
        }

        if (!string.IsNullOrEmpty(outcomeStatus))
        {
            query = query.Where(i => i.OutcomeStatus == outcomeStatus);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(i => i.InteractionAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
