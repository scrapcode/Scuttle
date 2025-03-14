
using Microsoft.AspNetCore.Mvc;
using Moq;
using MediatR;
using Scuttle.API.Controllers;
using Scuttle.Application.Users.Commands.Login;
using Scuttle.Application.Users.Commands.Register;

namespace Scuttle.API.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mockMediator = new Mock<IMediator>();
        _controller = new AuthController(_mockMediator.Object);
    }

    [Fact]
    public async Task Register_WithValidCommand_ReturnsOkResult()
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "password!123"
        };

        var expectedResult = new RegisterUserResult
        {
            UserId = Guid.NewGuid(),
            Username = command.Username
        };

        _mockMediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.Register(command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<RegisterUserResult>(okResult.Value);
        Assert.Equal(expectedResult.UserId, returnValue.UserId);
        Assert.Equal(expectedResult.Username, returnValue.Username);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsOkResult()
    {
        // Arrange
        var command = new LoginCommand
        {
            UsernameOrEmail = "testuser",
            Password = "password123"
        };

        var expectedResult = new LoginResult
        {
            Token = "jwt-token",
            Username = "testuser",
            Email = "test@example.com"
        };

        _mockMediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.Login(command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<LoginResult>(okResult.Value);
        Assert.Equal(expectedResult.Token, returnValue.Token);
        Assert.Equal(expectedResult.Username, returnValue.Username);
        Assert.Equal(expectedResult.Email, returnValue.Email);
    }
}
