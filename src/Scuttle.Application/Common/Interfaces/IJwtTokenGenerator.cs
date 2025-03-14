using Scuttle.Domain.Entities;

namespace Scuttle.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
