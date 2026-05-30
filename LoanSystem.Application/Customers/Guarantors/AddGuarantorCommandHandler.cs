using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Entities.Customers;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.Customers.Guarantors;

public sealed class AddGuarantorCommandHandler : ICommandHandler<AddGuarantorCommand, Guid>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddGuarantorCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(AddGuarantorCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdWithDetailsAsync(request.CustomerId, cancellationToken);
        if (customer is null)
        {
            return Result.Failure<Guid>(new Error("Customer.NotFound", "The specified customer does not exist."));
        }

        var guarantor = new Guarantor(
            customer.Id,
            request.Name,
            request.IdNumber,
            request.Phone,
            request.AmountGuaranteed,
            request.Relationship);

        customer.AddGuarantor(guarantor);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(guarantor.Id);
    }
}
