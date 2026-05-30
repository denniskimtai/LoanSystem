using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Entities.Customers;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.Customers.UpdateSecondaryInfo;

public sealed class UpdateCustomerSecondaryInfoCommandHandler : ICommandHandler<UpdateCustomerSecondaryInfoCommand>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCustomerSecondaryInfoCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateCustomerSecondaryInfoCommand request, CancellationToken cancellationToken)
    {
        // 1. Get customer with details (includes SecondaryInfo)
        var customer = await _customerRepository.GetByIdWithDetailsAsync(request.CustomerId, cancellationToken);
        if (customer is null)
        {
            return Result.Failure(new Error("Customer.NotFound", "The specified customer does not exist."));
        }

        // 2. Upsert SecondaryInfo
        if (customer.SecondaryInfo is not null)
        {
            customer.SecondaryInfo.Update(
                request.MaritalStatus,
                request.Dependents,
                request.Estate,
                request.HouseNumber,
                request.Ownership,
                request.RentAmount,
                request.HomeAssetValue,
                request.NearestLandmark,
                request.GeoLocation,
                request.HeardVia);
        }
        else
        {
            var secondaryInfo = new CustomerSecondaryInfo(
                customer.Id,
                request.MaritalStatus,
                request.Dependents,
                request.Estate,
                request.HouseNumber,
                request.Ownership,
                request.RentAmount,
                request.HomeAssetValue,
                request.NearestLandmark,
                request.GeoLocation,
                request.HeardVia);

            customer.SetSecondaryInfo(secondaryInfo);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
