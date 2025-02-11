using System.Security.Claims;
using MainProgram.Auth;

namespace MainProgram.Interfaces;

public interface ITokenService
{
    IEnumerable<Claim> GetClaims(Guid userId, int role, string email);
    Task<string> CreateToken(IEnumerable<Claim> claims, int tokenExpiresAfterHours = 0);
    Task<AuthResponse?> RefreshToken(string refreshToken);
}