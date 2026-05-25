using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.Customers.Referees;

public sealed class RemoveRefereeCommandHandler : ICommandHandler<RemoveRefereeCommand>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveRefereeCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RemoveRefereeCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdWithDetailsAsync(request.CustomerId, cancellationToken);
        if (customer is null)
        {
            return Result.Failure(new Error("Customer.NotFound", "The specified customer does not exist."));
        }

        var referee = customer.Referees.FirstOrDefault(r => r.Id == request.RefereeId);
        if (referee is null)
        {
            return Result.Failure(new Error("Customer.RefereeNotFound", "The specified referee does not exist on this customer."));
        }

        customer.RemoveReferee(request.RefereeId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
