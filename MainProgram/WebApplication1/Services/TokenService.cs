using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MainProgram.Interfaces;
using MainProgram.Repositories;
using Microsoft.IdentityModel.Tokens;
using MainProgram.Auth;
using MainProgram.Users;

namespace MainProgram.Services;

public class TokenService(IAuthSettings authSettings, IUserRepository userRepository) : ITokenService
{
    public IEnumerable<Claim> GetClaims(Guid userId, int role, string email)
    {
        var roleName = role == (int)Role.Author ? "Author" : "Reader"; 

        return new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
        new Claim(ClaimTypes.Role, roleName),
        new Claim(ClaimTypes.Email, email)
    };
    }

    public Task<string> CreateToken(IEnumerable<Claim> claims, int tokenExpiresAfterHours = 0)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Key));

        if (tokenExpiresAfterHours == 0)
        {
            tokenExpiresAfterHours = authSettings.TokenExpiresAfterHours;
        }

        var token = new JwtSecurityToken(
            authSettings.Issuer,
            authSettings.Audience,
            claims,
            null,
            DateTime.UtcNow.AddHours(tokenExpiresAfterHours),
            new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public async Task<AuthResponse?> RefreshToken(string refreshToken)
    {
        var user = await userRepository.GetUser(refreshToken);
        if (user == null)
        {
            return null; 
        }
        var claims = GetClaims(user.UserId, user.Role, user.Email);

        var newAccessToken = await CreateToken(claims, 24);
        var newRefreshToken = await CreateToken(new List<Claim>());

        return new AuthResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }
}