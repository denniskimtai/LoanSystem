using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Entities.Customers;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.Customers.Referees;

public sealed class AddRefereeCommandHandler : ICommandHandler<AddRefereeCommand, Guid>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddRefereeCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(AddRefereeCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer is null)
        {
            return Result.Failure<Guid>(new Error("Customer.NotFound", "The specified customer does not exist."));
        }

        var referee = new Referee(
            request.CustomerId,
            request.Name,
            request.Phone,
            request.PhysicalAddress,
            request.Relationship);

        customer.Referees.Add(referee);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(referee.Id);
    }
}
