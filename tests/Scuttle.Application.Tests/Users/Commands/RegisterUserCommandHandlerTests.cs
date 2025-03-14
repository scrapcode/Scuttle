using Moq;
using Scuttle.Application.Common.Interfaces;
using Scuttle.Application.Users.Commands.Register;
using Scuttle.Domain.Entities;
using Scuttle.Domain.Interfaces;

namespace Scuttle.Application.Tests.Users.Commands;
public class RegisterUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly RegisterUserCommandHandler _handler;

    public RegisterUserCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _handler = new RegisterUserCommandHandler(_mockUserRepository.Object, _mockPasswordHasher.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldRegisterUser()
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "StrongP@ssw0rd!"
        };

        string hashedPassword = "hashed_password";
        _mockPasswordHasher.Setup(p => p.HashPassword(command.Password)).Returns(hashedPassword);

        _mockUserRepository.Setup(r => r.ExistsByUsernameAsync(command.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _mockUserRepository.Setup(r => r.ExistsByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        User? addedUser = null;
        _mockUserRepository.Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Callback<User, CancellationToken>((u, _) => addedUser = u)
            .ReturnsAsync((User u, CancellationToken _) => u);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.UserId);

        _mockUserRepository.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotNull(addedUser);
        Assert.Equal(command.Username, addedUser.Username);
        Assert.Equal(command.Email, addedUser.Email);
        Assert.Equal(hashedPassword, addedUser.PasswordHash);
    }
}
