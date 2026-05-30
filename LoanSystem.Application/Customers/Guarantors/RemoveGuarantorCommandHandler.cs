using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.Customers.Guarantors;

public sealed class RemoveGuarantorCommandHandler : ICommandHandler<RemoveGuarantorCommand>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveGuarantorCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RemoveGuarantorCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdWithDetailsAsync(request.CustomerId, cancellationToken);
        if (customer is null)
        {
            return Result.Failure(new Error("Customer.NotFound", "The specified customer does not exist."));
        }

        var guarantor = customer.Guarantors.FirstOrDefault(g => g.Id == request.GuarantorId);
        if (guarantor is null)
        {
            return Result.Failure(new Error("Customer.GuarantorNotFound", "The specified guarantor does not exist on this customer."));
        }

        customer.RemoveGuarantor(request.GuarantorId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
