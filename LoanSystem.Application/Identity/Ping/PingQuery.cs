using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.Identity.Ping;

public sealed record PingQuery : IQuery<string>;
