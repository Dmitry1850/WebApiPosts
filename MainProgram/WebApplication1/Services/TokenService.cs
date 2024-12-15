using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MainProgram.Interfaces;
using MainProgram.Repositories;
using Microsoft.IdentityModel.Tokens;
using MainProgram.Auth;
using MainProgram.Common;

namespace EmployeeService.Services;

public class TokenService(IAuthSettings authSettings, IUserRepository userRepository) : ITokenService
{
    public string CreateToken(IEnumerable<Claim> claims, int tokenExpiresAfterHours = 0)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Key));

        if (tokenExpiresAfterHours == 0)
        {
            tokenExpiresAfterHours = authSettings.TokenExpiresAfterHours;
        }

        var token = new JwtSecurityToken(authSettings.Issuer, authSettings.Audience, claims, null, DateTime.UtcNow.AddHours(tokenExpiresAfterHours),
            new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<AuthResponse?> RefreshToken(string refreshToken)
    {
        var user = await userRepository.GetUser(refreshToken);
        if (user == null)
        {
            return null;
        }

        var claims = Jwt.GetClaims(user.UserId, user.Role, user.Email);
        var newRefreshToken = CreateToken(new List<Claim>());
        var newAccessToken = CreateToken(claims, 24);

        return new AuthResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }
}