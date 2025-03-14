using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Scuttle.Application.Common.Interfaces;
using Scuttle.Application.Users.Commands.Login;
using Scuttle.Domain.Entities;
using Scuttle.Domain.Interfaces;

namespace Scuttle.Application.Tests.Users.Commands;

public class LoginCommandHandlerTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly Mock<IJwtTokenGenerator> _mockJwtTokenGenerator;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _mockJwtTokenGenerator = new Mock<IJwtTokenGenerator>();
        _handler = new LoginCommandHandler(
            _mockUserRepository.Object,
            _mockPasswordHasher.Object,
            _mockJwtTokenGenerator.Object);
    }

    [Fact]
    public async Task Handle_WithValidUsernameAndPassword_ShouldReturnToken()
    {
        // Arrange
        var command = new LoginCommand
        {
            UsernameOrEmail = "testuser",
            Password = "correct_password"
        };

        var user = new User()
            .SetUsername("testuser")
            .SetEmail("test@example.com")
            .SetPasswordHash("hashed_password");

        _mockUserRepository.Setup(r => r.GetByUsernameAsync(command.UsernameOrEmail, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _mockPasswordHasher.Setup(h => h.VerifyPassword(command.Password, user.PasswordHash))
            .Returns(true);

        const string jwtToken = "jwt_token_string";
        _mockJwtTokenGenerator.Setup(g => g.GenerateToken(user))
            .Returns(jwtToken);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(jwtToken, result.Token);
        Assert.Equal(user.Username, result.Username);
        Assert.Equal(user.Email, result.Email);

        // Verify last login updated
        _mockUserRepository.Verify(r => r.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidEmailAndPassword_ShouldReturnToken()
    {
        // Arrange
        var command = new LoginCommand
        {
            UsernameOrEmail = "test@example.com",
            Password = "correct_password"
        };

        var user = new User()
            .SetUsername("testuser")
            .SetEmail("test@example.com")
            .SetPasswordHash("hashed_password");

        _mockUserRepository.Setup(r => r.GetByEmailAsync(command.UsernameOrEmail, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _mockPasswordHasher.Setup(h => h.VerifyPassword(command.Password, user.PasswordHash))
            .Returns(true);

        const string jwtToken = "jwt_token_string";
        _mockJwtTokenGenerator.Setup(g => g.GenerateToken(user))
            .Returns(jwtToken);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(jwtToken, result.Token);
        Assert.Equal(user.Username, result.Username);
        Assert.Equal(user.Email, result.Email);

        // Verify last login updated
        _mockUserRepository.Verify(r => r.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_UserWithInvalidUsername_ShouldThrowException()
    {
        // Arrange
        var command = new LoginCommand
        {
            UsernameOrEmail = "nonexistent",
            Password = "some_password"
        };

        _mockUserRepository.Setup(r => r.GetByUsernameAsync(command.UsernameOrEmail, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ApplicationException>(() =>
            _handler.Handle(command, CancellationToken.None));

        Assert.Contains("Invalid username/email or password", exception.Message);
    }

    [Fact]
    public async Task Handle_WithInvalidPassword_ShouldThrowException()
    {
        // Arrange
        var command = new LoginCommand
        {
            UsernameOrEmail = "testuser",
            Password = "wrong_password"
        };

        var user = new User()
            .SetUsername("testuser")
            .SetEmail("test@example.com")
            .SetPasswordHash("hashed_password");

        _mockUserRepository.Setup(r => r.GetByUsernameAsync(command.UsernameOrEmail, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _mockPasswordHasher.Setup(p => p.VerifyPassword(command.Password, user.PasswordHash))
            .Returns(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ApplicationException>(() =>
            _handler.Handle(command, CancellationToken.None));

        Assert.Contains("Invalid username/email or password", exception.Message);
    }
}
