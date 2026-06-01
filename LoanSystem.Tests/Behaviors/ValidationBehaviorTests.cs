using FluentValidation;
using LoanSystem.Application.Behaviors;
using LoanSystem.Domain.Primitives;
using MediatR;
using NSubstitute;

namespace LoanSystem.Tests.Behaviors;

public class ValidationBehaviorTests
{
    public sealed record TestRequest : IRequest<Result<string>>;

    [Fact]
    public async Task Handle_Should_ReturnValidationResult_WhenValidationFails()
    {
        // Arrange
        var validator = Substitute.For<IValidator<TestRequest>>();
        var validationResult = new FluentValidation.Results.ValidationResult(new[]
        {
            new FluentValidation.Results.ValidationFailure("Prop", "Some error message")
        });

        validator.ValidateAsync(Arg.Any<ValidationContext<TestRequest>>(), Arg.Any<CancellationToken>())
            .Returns(validationResult);

        var behavior = new ValidationBehavior<TestRequest, Result<string>>(new[] { validator });

        var nextCalled = false;
        RequestHandlerDelegate<Result<string>> next = (cancellationToken) =>
        {
            nextCalled = true;
            return Task.FromResult(Result.Success("Ok"));
        };

        // Act
        var result = await behavior.Handle(new TestRequest(), next, CancellationToken.None);

        // Assert
        Assert.False(nextCalled);
        Assert.True(result.IsFailure);
        Assert.IsType<ValidationError>(result.Error);
        var validationError = (ValidationError)result.Error;
        Assert.Contains("Some error message", validationError.Errors);
    }
}
