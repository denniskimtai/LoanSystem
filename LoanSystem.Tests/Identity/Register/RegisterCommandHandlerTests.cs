using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Application.Identity.Register;
using LoanSystem.Domain.Entities.Identity;
using LoanSystem.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace LoanSystem.Tests.Identity.Register;

public class RegisterCommandHandlerTests
{
    private readonly UserManager<User> _userManager;
    private readonly IBranchRepository _branchRepository;
    private readonly RegisterCommandHandler _handler;

    public RegisterCommandHandlerTests()
    {
        var store = Substitute.For<IUserStore<User>>();
        _userManager = Substitute.For<UserManager<User>>(store, null, null, null, null, null, null, null, null);
        _branchRepository = Substitute.For<IBranchRepository>();
        _handler = new RegisterCommandHandler(_userManager, _branchRepository);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenRegistrationIsSuccessful()
    {
        // Arrange
        var command = new RegisterCommand("test@test.com", "Password123!", "Test User", "LoanOfficer", Guid.NewGuid());
        
        _branchRepository.GetByIdAsync(command.BranchId, Arg.Any<CancellationToken>())
            .Returns(new Branch("Main Branch", "Main Location"));

        _userManager.FindByEmailAsync(command.Email)
            .Returns((User?)null);

        _userManager.CreateAsync(Arg.Any<User>(), command.Password)
            .Returns(IdentityResult.Success);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenEmailAlreadyExists()
    {
        // Arrange
        var command = new RegisterCommand("test@test.com", "Password123!", "Test User", "LoanOfficer", Guid.NewGuid());
        
        _branchRepository.GetByIdAsync(command.BranchId, Arg.Any<CancellationToken>())
            .Returns(new Branch("Main Branch", "Main Location"));

        _userManager.FindByEmailAsync(command.Email)
            .Returns(new User(command.Email, command.FullName, UserRole.LoanOfficer, command.BranchId));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Identity.DuplicateEmail", result.Error.Code);
    }
}
