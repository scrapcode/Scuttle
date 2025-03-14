using MediatR;

namespace Scuttle.Application.Users.Commands.Login;

public class LoginCommand : IRequest<LoginResult>
{
    public required string UsernameOrEmail { get; set; }
    public required string Password { get; set; }
}

public class LoginResult
{
    public required string Token { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
}
