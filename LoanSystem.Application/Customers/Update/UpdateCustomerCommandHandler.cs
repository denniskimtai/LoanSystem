using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.Customers.Update;

public sealed class UpdateCustomerCommandHandler : ICommandHandler<UpdateCustomerCommand>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        // 1. Get customer
        var customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);
        if (customer is null)
        {
            return Result.Failure(new Error("Customer.NotFound", "The specified customer does not exist."));
        }

        // 2. Validate National ID uniqueness if changed
        if (request.NationalId != customer.NationalId)
        {
            if (await _customerRepository.ExistsByNationalIdAsync(request.NationalId, cancellationToken))
            {
                return Result.Failure(new Error("Customer.DuplicateNationalId", "A customer with this National ID already exists."));
            }
        }

        // 3. Validate Phone uniqueness if changed
        if (request.Phone != customer.Phone)
        {
            if (await _customerRepository.ExistsByPhoneAsync(request.Phone, cancellationToken))
            {
                return Result.Failure(new Error("Customer.DuplicatePhone", "A customer with this Phone number already exists."));
            }
        }

        // 4. Update basic info
        customer.UpdateBasicInfo(
            request.FullName,
            request.NationalId,
            request.Phone,
            request.PhotoUrl,
            request.PhysicalAddress,
            request.HomeGeoLocation,
            request.Town,
            request.County,
            request.PostalAddress);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
