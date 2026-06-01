using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Entities.Customers;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.Customers.Create;

public sealed class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, Guid>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IBranchRepository branchRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _branchRepository = branchRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        // 1. Verify branch exists
        var branch = await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken);
        if (branch is null)
        {
            return Result.Failure<Guid>(new Error("Customer.BranchNotFound", "The specified branch does not exist."));
        }

        // 2. Check duplicate National ID
        if (await _customerRepository.ExistsByNationalIdAsync(request.NationalId, cancellationToken))
        {
            return Result.Failure<Guid>(new Error("Customer.DuplicateNationalId", "A customer with this National ID already exists."));
        }

        // 3. Check duplicate Phone
        if (await _customerRepository.ExistsByPhoneAsync(request.Phone, cancellationToken))
        {
            return Result.Failure<Guid>(new Error("Customer.DuplicatePhone", "A customer with this Phone number already exists."));
        }

        // 4. Create basic customer entity
        var customer = new Customer(
            request.FullName,
            request.NationalId,
            request.Phone,
            request.PhotoUrl ?? string.Empty,
            request.PhysicalAddress,
            request.HomeGeoLocation ?? string.Empty,
            request.Town,
            request.County,
            request.PostalAddress,
            request.BranchId,
            request.CreatedById);

        // 5. Add Business Info if provided
        if (request.BusinessInfo is not null)
        {
            var businessInfo = new CustomerBusinessInfo(
                customer.Id,
                request.BusinessInfo.BusinessName,
                request.BusinessInfo.BusinessType,
                request.BusinessInfo.BusinessDirection,
                request.BusinessInfo.BusinessGeoLocation,
                request.BusinessInfo.CurrentStockValue,
                request.BusinessInfo.WeeklyGrossProfit,
                request.BusinessInfo.WeeklyNetProfit,
                request.BusinessInfo.WeeklyExpenses,
                request.BusinessInfo.YearsInBusiness,
                request.BusinessInfo.OffersCredit,
                request.BusinessInfo.LeadType,
                request.BusinessInfo.ProposedLimit,
                request.BusinessInfo.WouldLend);

            customer.SetBusinessInfo(businessInfo);
        }

        // 6. Add Secondary Info if provided
        if (request.SecondaryInfo is not null)
        {
            var secondaryInfo = new CustomerSecondaryInfo(
                customer.Id,
                request.SecondaryInfo.MaritalStatus,
                request.SecondaryInfo.Dependents,
                request.SecondaryInfo.Estate,
                request.SecondaryInfo.HouseNumber,
                request.SecondaryInfo.Ownership,
                request.SecondaryInfo.RentAmount,
                request.SecondaryInfo.HomeAssetValue,
                request.SecondaryInfo.NearestLandmark,
                request.SecondaryInfo.GeoLocation,
                request.SecondaryInfo.HeardVia);

            customer.SetSecondaryInfo(secondaryInfo);
        }

        // 7. Add Guarantors if provided
        if (request.Guarantors is not null)
        {
            foreach (var gInput in request.Guarantors)
            {
                var guarantor = new Guarantor(
                    customer.Id,
                    gInput.Name,
                    gInput.IdNumber,
                    gInput.Phone,
                    gInput.AmountGuaranteed,
                    gInput.Relationship);

                customer.AddGuarantor(guarantor);
            }
        }

        // 8. Add Referees if provided
        if (request.Referees is not null)
        {
            foreach (var rInput in request.Referees)
            {
                var referee = new Referee(
                    customer.Id,
                    rInput.Name,
                    rInput.Phone,
                    rInput.PhysicalAddress,
                    rInput.Relationship);

                customer.AddReferee(referee);
            }
        }

        // 9. Save
        _customerRepository.Add(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(customer.Id);
    }
}
