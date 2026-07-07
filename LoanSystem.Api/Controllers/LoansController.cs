using LoanSystem.Api.Loans;
using LoanSystem.Application.Loans.Approve;
using LoanSystem.Application.Loans.Create;
using LoanSystem.Application.Loans.Delete;
using LoanSystem.Application.Loans.Disburse;
using LoanSystem.Application.Loans.GetById;
using LoanSystem.Application.Loans.GetPaged;
using LoanSystem.Application.Loans.Reject;
using LoanSystem.Application.Loans.Update;
using LoanSystem.Domain.Entities.Identity;
using LoanSystem.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoanSystem.Api.Controllers;

[Route("api/loans")]
[Authorize]
public sealed class LoansController : ApiController
{
    public LoansController(ISender sender) : base(sender)
    {
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager},{Roles.LoanOfficer}")]
    public async Task<IActionResult> Create([FromBody] CreateLoanRequest request, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized();
        }

        var command = new CreateLoanCommand(
            request.CustomerId,
            request.ProductId,
            request.LoId,
            request.CoId,
            userId,
            request.Principal,
            request.InterestAmount,
            request.Type,
            request.Addons,
            request.Deductions);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager},{Roles.LoanOfficer}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLoanRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateLoanCommand(
            id,
            request.Principal,
            request.InterestAmount,
            request.ProductId,
            request.LoId,
            request.CoId,
            request.Type);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteLoanCommand(id);
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok();
    }

    [HttpPost("{id:guid}/approve")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager},{Roles.CollectionOfficer}")]
    public async Task<IActionResult> Approve(Guid id, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized();
        }

        var command = new ApproveLoanCommand(id, userId);
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok();
    }

    [HttpPost("{id:guid}/disburse")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager},{Roles.LoanOfficer}")]
    public async Task<IActionResult> Disburse(Guid id, [FromBody] DisburseLoanRequest request, CancellationToken cancellationToken)
    {
        var command = new DisburseLoanCommand(
            id,
            request.MpesaCode,
            request.DisbursedAt);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok();
    }

    [HttpPost("{id:guid}/reject")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager},{Roles.CollectionOfficer}")]
    public async Task<IActionResult> Reject(Guid id, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized();
        }

        var command = new RejectLoanCommand(id, userId);
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
        var query = new GetLoanByIdQuery(id);
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
        [FromQuery] LoanStatus? status = null,
        [FromQuery] LoanStage? stage = null,
        [FromQuery] Guid? customerId = null,
        [FromQuery] Guid? branchId = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetLoansQuery(
            page,
            pageSize,
            searchTerm,
            status,
            stage,
            customerId,
            branchId);

        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }
}
