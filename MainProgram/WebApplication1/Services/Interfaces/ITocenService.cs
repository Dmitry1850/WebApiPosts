using MainProgram.Auth;
using System.Security.Claims;

namespace MainProgram.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(IEnumerable<Claim> claims, int tokenExpiresAfterHours = 0);
        Task<AuthResponse?> RefreshToken(string refreshToken);
    }
}
