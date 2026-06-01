using LoanSystem.Api.CRM;
using LoanSystem.Application.CRM.Interactions.Create;
using LoanSystem.Application.CRM.Interactions.Delete;
using LoanSystem.Application.CRM.Interactions.GetById;
using LoanSystem.Application.CRM.Interactions.GetPaged;
using LoanSystem.Application.CRM.Interactions.Update;
using LoanSystem.Domain.Entities.Identity;
using LoanSystem.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoanSystem.Api.Controllers;

[Route("api/interactions")]
[Authorize]
public sealed class InteractionsController : ApiController
{
    public InteractionsController(ISender sender) : base(sender)
    {
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager},{Roles.LoanOfficer}")]
    public async Task<IActionResult> Create([FromBody] CreateInteractionRequest request, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized();
        }

        var command = new CreateInteractionCommand(
            request.CustomerId,
            userId,
            request.Mode,
            request.Purpose,
            request.OutcomeDetails,
            request.OutcomeStatus,
            request.Tag,
            request.DefaultReason,
            request.NextSteps,
            request.LocationGeo,
            request.InteractionAt,
            request.LoanId,
            request.PromisedAmount,
            request.NextInteractionDate);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager},{Roles.LoanOfficer}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateInteractionRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateInteractionCommand(
            id,
            request.Mode,
            request.Purpose,
            request.OutcomeDetails,
            request.OutcomeStatus,
            request.Tag,
            request.DefaultReason,
            request.NextSteps,
            request.LocationGeo,
            request.InteractionAt,
            request.LoanId,
            request.PromisedAmount,
            request.NextInteractionDate);

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
        var command = new DeleteInteractionCommand(id);
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
        var query = new GetInteractionByIdQuery(id);
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
        [FromQuery] Guid? customerId = null,
        [FromQuery] Guid? agentId = null,
        [FromQuery] string? tag = null,
        [FromQuery] string? outcomeStatus = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetInteractionsQuery(page, pageSize, customerId, agentId, tag, outcomeStatus);
        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }
}
