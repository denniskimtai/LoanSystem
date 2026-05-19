using LoanSystem.Domain.Primitives;
using MediatR;

namespace LoanSystem.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
