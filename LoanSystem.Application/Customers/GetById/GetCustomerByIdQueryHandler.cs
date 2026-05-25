using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.Customers.GetById;

public sealed class GetCustomerByIdQueryHandler : IQueryHandler<GetCustomerByIdQuery, CustomerDetailsResponse>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Result<CustomerDetailsResponse>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);
        if (customer is null)
        {
            return Result.Failure<CustomerDetailsResponse>(new Error("Customer.NotFound", "The specified customer does not exist."));
        }

        var businessResponse = customer.BusinessInfo is null ? null : new CustomerBusinessInfoResponse(
            customer.BusinessInfo.Id,
            customer.BusinessInfo.BusinessName,
            customer.BusinessInfo.BusinessType,
            customer.BusinessInfo.BusinessDirection,
            customer.BusinessInfo.BusinessGeoLocation,
            customer.BusinessInfo.CurrentStockValue,
            customer.BusinessInfo.WeeklyGrossProfit,
            customer.BusinessInfo.WeeklyNetProfit,
            customer.BusinessInfo.WeeklyExpenses,
            customer.BusinessInfo.YearsInBusiness,
            customer.BusinessInfo.OffersCredit,
            customer.BusinessInfo.LeadType,
            customer.BusinessInfo.ProposedLimit,
            customer.BusinessInfo.WouldLend,
            customer.BusinessInfo.CreatedAt,
            customer.BusinessInfo.UpdatedAt);

        var secondaryResponse = customer.SecondaryInfo is null ? null : new CustomerSecondaryInfoResponse(
            customer.SecondaryInfo.Id,
            customer.SecondaryInfo.MaritalStatus,
            customer.SecondaryInfo.Dependents,
            customer.SecondaryInfo.Estate,
            customer.SecondaryInfo.HouseNumber,
            customer.SecondaryInfo.Ownership,
            customer.SecondaryInfo.RentAmount,
            customer.SecondaryInfo.HomeAssetValue,
            customer.SecondaryInfo.NearestLandmark,
            customer.SecondaryInfo.GeoLocation,
            customer.SecondaryInfo.HeardVia,
            customer.SecondaryInfo.CreatedAt,
            customer.SecondaryInfo.UpdatedAt);

        var guarantorsResponse = customer.Guarantors.Select(g => new GuarantorResponse(
            g.Id,
            g.Name,
            g.IdNumber,
            g.Phone,
            g.AmountGuaranteed,
            g.Relationship,
            g.CreatedAt,
            g.UpdatedAt)).ToList();

        var refereesResponse = customer.Referees.Select(r => new RefereeResponse(
            r.Id,
            r.Name,
            r.Phone,
            r.PhysicalAddress,
            r.Relationship,
            r.CreatedAt,
            r.UpdatedAt)).ToList();

        var response = new CustomerDetailsResponse(
            customer.Id,
            customer.FullName,
            customer.NationalId,
            customer.Phone,
            customer.PhotoUrl,
            customer.Status,
            customer.PhysicalAddress,
            customer.HomeGeoLocation,
            customer.Town,
            customer.County,
            customer.PostalAddress,
            customer.CurrentLimit,
            customer.CurrentLoId,
            customer.CurrentCoId,
            customer.BranchId,
            customer.CreatedAt,
            customer.UpdatedAt,
            businessResponse,
            secondaryResponse,
            guarantorsResponse,
            refereesResponse);

        return Result.Success(response);
    }
}
