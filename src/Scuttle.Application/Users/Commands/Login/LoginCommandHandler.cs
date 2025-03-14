
using MediatR;
using Scuttle.Application.Common.Interfaces;
using Scuttle.Domain.Entities;
using Scuttle.Domain.Interfaces;

namespace Scuttle.Application.Users.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _jwtTokenGenerator = jwtTokenGenerator ?? throw new ArgumentNullException(nameof(jwtTokenGenerator));
    }

    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        User? user = null;

        if (request.UsernameOrEmail.Contains('@'))
        {
            user = await _userRepository.GetByEmailAsync(request.UsernameOrEmail);
        }
        else
        {
            user = await _userRepository.GetByUsernameAsync(request.UsernameOrEmail);
        }

        // Check if user exists
        if (user == null)
        {
            throw new Common.Exceptions.AuthenticationException("Invalid username/email or password");
        }

        // Verify password
        if(!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new Common.Exceptions.AuthenticationException("Invalid username/email or password");
        }

        // Update last login time
        user.UpdateLastLogin();
        await _userRepository.UpdateAsync(user, cancellationToken);

        // Generate JWT
        string token = _jwtTokenGenerator.GenerateToken(user);

        return new LoginResult
        {
            Token = token,
            Username = user.Username,
            Email = user.Email
        };
    }
}
