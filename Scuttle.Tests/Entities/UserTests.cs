using System;

using Scuttle.Domain.Entities;

using Xunit;

namespace Scuttle.Domain.Tests.Entities;

public class UserTests
{
    [Fact]
    public void User_WithValidProperties_ShouldCreateSuccessfully()
    {
        // Arrange & Act
        var user = new User
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };

        user.SetUsername("testuser");
        user.SetEmail("test@example.com");

        // Assert
        Assert.NotNull(user);
        Assert.Equal("testuser", user.Username);
        Assert.Equal("test@example.com", user.Email);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void User_WithInvalidUsername_ShouldThrowArgumentException(string? invalidUsername)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new User().SetUsername(invalidUsername!));

        Assert.Contains("Username", exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    [InlineData("invalid-email")]
    public void User_WithInvalidEmail_ShouldThrowArgumentException(string? invalidEmail)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new User().SetEmail(invalidEmail!));

        Assert.Contains("Email", exception.Message);
    }
}
