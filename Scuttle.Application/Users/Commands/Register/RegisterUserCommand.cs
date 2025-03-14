using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Scuttle.Application.Users.Commands.Register;

public class RegisterUserCommand : IRequest<RegisterUserResult>
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class RegisterUserResult
{
    public Guid UserId { get; set; }
    public required string Username { get; set; }
}
