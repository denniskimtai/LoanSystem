using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.Branches.GetById;

public sealed record GetBranchByIdQuery(Guid Id) : IQuery<BranchResponse>;
