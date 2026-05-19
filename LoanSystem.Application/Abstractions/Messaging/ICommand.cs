using LoanSystem.Domain.Primitives;
using MediatR;

namespace LoanSystem.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
