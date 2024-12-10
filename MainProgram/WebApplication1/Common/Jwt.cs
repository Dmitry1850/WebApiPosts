using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MainProgram.Common
{
    public static class Jwt
    {
        public static string? GetId(string token)
        {
            return ParseToken(token, "UserID");
        }
    }
}
