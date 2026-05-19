using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.Identity.Ping;

internal sealed class PingQueryHandler : IQueryHandler<PingQuery, string>
{
    public Task<Result<string>> Handle(PingQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Result.Success("Pong! The LoanSystem API is running and MediatR pipeline is fully functional."));
    }
}
