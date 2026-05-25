using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Application.Branches.Update;
using LoanSystem.Domain.Entities.Identity;
using NSubstitute;

namespace LoanSystem.Tests.Branches;

public class UpdateBranchCommandHandlerTests
{
    private readonly IBranchRepository _branchRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UpdateBranchCommandHandler _handler;

    public UpdateBranchCommandHandlerTests()
    {
        _branchRepository = Substitute.For<IBranchRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new UpdateBranchCommandHandler(_branchRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenUpdateIsSuccessful()
    {
        // Arrange
        var command = new UpdateBranchCommand(Guid.NewGuid(), "Nakuru East", "CBD Office");
        var branch = new Branch("Nakuru", "Westside Mall");

        _branchRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(branch);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Nakuru East", branch.Name);
        Assert.Equal("CBD Office", branch.Location);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenBranchDoesNotExist()
    {
        // Arrange
        var command = new UpdateBranchCommand(Guid.NewGuid(), "Nakuru East", "CBD Office");

        _branchRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((Branch?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Branch.NotFound", result.Error.Code);
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
