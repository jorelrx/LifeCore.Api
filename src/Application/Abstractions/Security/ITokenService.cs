using LifeOS.Domain.Entities;

namespace LifeOS.Application.Abstractions.Security;

public interface ITokenService
{
    TokenPair GenerateTokens(User user);
}