using LifeCore.Domain.Entities;

namespace LifeCore.Application.Abstractions.Security;

public interface ITokenService
{
    TokenPair GenerateTokens(User user);
}