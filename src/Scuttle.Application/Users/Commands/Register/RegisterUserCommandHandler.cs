using MediatR;
using Scuttle.Application.Common.Interfaces;
using Scuttle.Domain.Entities;
using Scuttle.Domain.Interfaces;

namespace Scuttle.Application.Users.Commands.Register;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
    }

    public async Task<RegisterUserResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // Validate our fields aren't blank.
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            throw new ApplicationException("Required input is missing.");

        // Validate that the username is unique
        if (await _userRepository.ExistsByUsernameAsync(request.Username))
            throw new ApplicationException($"Username already exists: {request.Username}");

        // Validate that the email is unique
        if (await _userRepository.ExistsByEmailAsync(request.Email))
            throw new ApplicationException($"Email already exists: {request.Email}");

        // Hash the password
        string passwordHash = _passwordHasher.HashPassword(request.Password);

        // Create the User
        var user = new User()
            .SetUsername(request.Username)
            .SetPasswordHash(passwordHash)
            .SetEmail(request.Email);

        var savedUser = await _userRepository.AddAsync(user, cancellationToken);

        return new RegisterUserResult
        {
            UserId = savedUser.Id,
            Username = savedUser.Username
        };
    }
}
