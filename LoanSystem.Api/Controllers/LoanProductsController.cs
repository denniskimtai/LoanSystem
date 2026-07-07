using LoanSystem.Domain.Entities.Identity;
using LoanSystem.Application.LoanProducts.Create;
using LoanSystem.Application.LoanProducts.Delete;
using LoanSystem.Application.LoanProducts.GetById;
using LoanSystem.Application.LoanProducts.GetPaged;
using LoanSystem.Application.LoanProducts.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LoanSystem.Api.LoanProducts;

namespace LoanSystem.Api.Controllers;

[Route("api/loan-products")]
public sealed class LoanProductsController : ApiController
{
    public LoanProductsController(ISender sender) : base(sender)
    {
    }

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Create([FromBody] CreateLoanProductCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLoanProductRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateLoanProductCommand(
            id,
            request.Name,
            request.MinAmount,
            request.MaxAmount,
            request.InterestRate,
            request.RepaymentDays);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteLoanProductCommand(id);
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok();
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetLoanProductByIdQuery(id);
        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetLoanProductsQuery(page, pageSize, searchTerm);
        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }
}
