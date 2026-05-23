using LoanSystem.Application.Abstractions.Identity;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Application.Identity.Login;
using LoanSystem.Domain.Entities.Identity;
using LoanSystem.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace LoanSystem.Tests.Identity.Login;

public class LoginCommandHandlerTests
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtProvider _jwtProvider;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        var store = Substitute.For<IUserStore<User>>();
        _userManager = Substitute.For<UserManager<User>>(store, null, null, null, null, null, null, null, null);
        _jwtProvider = Substitute.For<IJwtProvider>();
        _refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new LoginCommandHandler(_userManager, _jwtProvider, _refreshTokenRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenCredentialsAreValid()
    {
        // Arrange
        var command = new LoginCommand("test@test.com", "Password123!", "127.0.0.1");
        var user = new User(command.Email, "Test User", UserRole.LoanOfficer, Guid.NewGuid());
        
        _userManager.FindByEmailAsync(command.Email)
            .Returns(user);

        _userManager.CheckPasswordAsync(user, command.Password)
            .Returns(true);

        _jwtProvider.GenerateToken(user)
            .Returns("fake_jwt_token");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("fake_jwt_token", result.Value.AccessToken);
        Assert.NotNull(result.Value.RefreshToken);
        _refreshTokenRepository.Received(1).Add(Arg.Any<RefreshToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenPasswordIsInvalid()
    {
        // Arrange
        var command = new LoginCommand("test@test.com", "WrongPassword", "127.0.0.1");
        var user = new User(command.Email, "Test User", UserRole.LoanOfficer, Guid.NewGuid());
        
        _userManager.FindByEmailAsync(command.Email)
            .Returns(user);

        _userManager.CheckPasswordAsync(user, command.Password)
            .Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Identity.InvalidCredentials", result.Error.Code);
    }
}
