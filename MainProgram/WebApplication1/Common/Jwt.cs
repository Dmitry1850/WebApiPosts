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

        private static string? ParseToken(string token, string type)
        {
            if (token.Contains("Bearer"))
            {
                token = token.Split(' ')[1];
            }

            var handler = new JwtSecurityTokenHandler();
            var payload = handler.ReadJwtToken(token).Payload;

            return payload.Claims.FirstOrDefault(c => c.Type.Split('/').Last() == type)?.Value; // поиск коллекций утверждения calms (переменная type)
        }
    }
}
