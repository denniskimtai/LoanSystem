using LoanSystem.Api.Customers;
using LoanSystem.Application.Customers.Create;
using LoanSystem.Application.Customers.Delete;
using LoanSystem.Application.Customers.GetById;
using LoanSystem.Application.Customers.GetPaged;
using LoanSystem.Application.Customers.Update;
using LoanSystem.Application.Customers.UpdateBusinessInfo;
using LoanSystem.Application.Customers.UpdateSecondaryInfo;
using LoanSystem.Application.Customers.Guarantors;
using LoanSystem.Application.Customers.Referees;
using LoanSystem.Domain.Entities.Identity;
using LoanSystem.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoanSystem.Api.Controllers;

[Route("api/customers")]
[Authorize]
public sealed class CustomersController : ApiController
{
    public CustomersController(ISender sender) : base(sender)
    {
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager},{Roles.LoanOfficer}")]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized();
        }

        var command = new CreateCustomerCommand(
            request.FullName,
            request.NationalId,
            request.Phone,
            request.PhotoUrl,
            request.PhysicalAddress,
            request.HomeGeoLocation,
            request.Town,
            request.County,
            request.PostalAddress,
            request.BranchId,
            userId,
            request.BusinessInfo,
            request.SecondaryInfo,
            request.Guarantors,
            request.Referees);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager},{Roles.LoanOfficer}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCustomerCommand(
            id,
            request.FullName,
            request.NationalId,
            request.Phone,
            request.PhotoUrl,
            request.PhysicalAddress,
            request.HomeGeoLocation,
            request.Town,
            request.County,
            request.PostalAddress);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok();
    }

    [HttpPut("{id:guid}/business-info")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager},{Roles.LoanOfficer}")]
    public async Task<IActionResult> UpdateBusinessInfo(Guid id, [FromBody] UpdateCustomerBusinessInfoRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCustomerBusinessInfoCommand(
            id,
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

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok();
    }

    [HttpPut("{id:guid}/secondary-info")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager},{Roles.LoanOfficer}")]
    public async Task<IActionResult> UpdateSecondaryInfo(Guid id, [FromBody] UpdateCustomerSecondaryInfoRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCustomerSecondaryInfoCommand(
            id,
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

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok();
    }

    [HttpPost("{id:guid}/guarantors")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager},{Roles.LoanOfficer}")]
    public async Task<IActionResult> AddGuarantor(Guid id, [FromBody] AddGuarantorRequest request, CancellationToken cancellationToken)
    {
        var command = new AddGuarantorCommand(
            id,
            request.Name,
            request.IdNumber,
            request.Phone,
            request.AmountGuaranteed,
            request.Relationship);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}/guarantors/{guarantorId:guid}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager},{Roles.LoanOfficer}")]
    public async Task<IActionResult> RemoveGuarantor(Guid id, Guid guarantorId, CancellationToken cancellationToken)
    {
        var command = new RemoveGuarantorCommand(id, guarantorId);
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok();
    }

    [HttpPost("{id:guid}/referees")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager},{Roles.LoanOfficer}")]
    public async Task<IActionResult> AddReferee(Guid id, [FromBody] AddRefereeRequest request, CancellationToken cancellationToken)
    {
        var command = new AddRefereeCommand(
            id,
            request.Name,
            request.Phone,
            request.PhysicalAddress,
            request.Relationship);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}/referees/{refereeId:guid}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager},{Roles.LoanOfficer}")]
    public async Task<IActionResult> RemoveReferee(Guid id, Guid refereeId, CancellationToken cancellationToken)
    {
        var command = new RemoveRefereeCommand(id, refereeId);
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager},{Roles.LoanOfficer}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteCustomerCommand(id);
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetCustomerByIdQuery(id);
        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] CustomerStatus? status = null,
        [FromQuery] Guid? branchId = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetCustomersQuery(page, pageSize, searchTerm, status, branchId);
        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }
}
