using LoanSystem.Domain.Primitives;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LoanSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiController : ControllerBase
{
    protected readonly ISender Sender;

    protected ApiController(ISender sender)
    {
        Sender = sender;
    }

    protected IActionResult HandleFailure(Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Success result cannot be treated as a failure.");
        }

        if (result.Error is ValidationError validationError)
        {
            return BadRequest(new
            {
                Title = "Validation Error",
                Status = StatusCodes.Status400BadRequest,
                Errors = validationError.Errors
            });
        }

        return BadRequest(new
        {
            Title = "Bad Request",
            Status = StatusCodes.Status400BadRequest,
            Error = result.Error
        });
    }
}
