using Shop.Api.Domain.Entities;

namespace Shop.Api.Abstractions.Authentication;

public interface IJwtProvider
{
    public string GenerateToken(
        ApplicationUser user, 
        IList<string> roles);
}
