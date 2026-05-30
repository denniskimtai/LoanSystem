using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.Customers.Delete;

public sealed class DeleteCustomerCommandHandler : ICommandHandler<DeleteCustomerCommand>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);
        if (customer is null)
        {
            return Result.Failure(new Error("Customer.NotFound", "The specified customer does not exist."));
        }

        customer.MarkAsDeleted();

        if (customer.BusinessInfo is not null)
        {
            customer.BusinessInfo.MarkAsDeleted();
        }

        if (customer.SecondaryInfo is not null)
        {
            customer.SecondaryInfo.MarkAsDeleted();
        }

        foreach (var guarantor in customer.Guarantors)
        {
            guarantor.MarkAsDeleted();
        }

        foreach (var referee in customer.Referees)
        {
            referee.MarkAsDeleted();
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
