using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Application.Branches.Create;
using LoanSystem.Domain.Entities.Identity;
using NSubstitute;

namespace LoanSystem.Tests.Branches;

public class CreateBranchCommandHandlerTests
{
    private readonly IBranchRepository _branchRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateBranchCommandHandler _handler;

    public CreateBranchCommandHandlerTests()
    {
        _branchRepository = Substitute.For<IBranchRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CreateBranchCommandHandler(_branchRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenCreateIsSuccessful()
    {
        // Arrange
        var command = new CreateBranchCommand("Nakuru", "Westside Mall");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value);
        _branchRepository.Received(1).Add(Arg.Any<Branch>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
