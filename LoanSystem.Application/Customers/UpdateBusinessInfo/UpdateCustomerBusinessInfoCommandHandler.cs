using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Entities.Customers;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.Customers.UpdateBusinessInfo;

public sealed class UpdateCustomerBusinessInfoCommandHandler : ICommandHandler<UpdateCustomerBusinessInfoCommand>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCustomerBusinessInfoCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateCustomerBusinessInfoCommand request, CancellationToken cancellationToken)
    {
        // 1. Get customer with details (includes BusinessInfo)
        var customer = await _customerRepository.GetByIdWithDetailsAsync(request.CustomerId, cancellationToken);
        if (customer is null)
        {
            return Result.Failure(new Error("Customer.NotFound", "The specified customer does not exist."));
        }

        // 2. Upsert BusinessInfo
        if (customer.BusinessInfo is not null)
        {
            customer.BusinessInfo.Update(
                request.BusinessName,
                request.BusinessType,
                request.BusinessDirection,
                request.BusinessGeoLocation,
                request.CurrentStockValue,
                request.WeeklyGrossProfit,
                request.WeeklyNetProfit,
                request.WeeklyExpenses,
                request.YearsInBusiness,
                request.OffersCredit,
                request.LeadType,
                request.ProposedLimit,
                request.WouldLend);
        }
        else
        {
            var businessInfo = new CustomerBusinessInfo(
                customer.Id,
                request.BusinessName,
                request.BusinessType,
                request.BusinessDirection,
                request.BusinessGeoLocation,
                request.CurrentStockValue,
                request.WeeklyGrossProfit,
                request.WeeklyNetProfit,
                request.WeeklyExpenses,
                request.YearsInBusiness,
                request.OffersCredit,
                request.LeadType,
                request.ProposedLimit,
                request.WouldLend);

            customer.SetBusinessInfo(businessInfo);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
