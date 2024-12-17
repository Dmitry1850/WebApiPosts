using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MainProgram.Common
{
    public static class Jwt
    {
        public static string? GetId(string token)
        {
            return ParseToken(token, "userId");
        }

        public static string? GetRole(string token)
        {
            return ParseToken(token, "role");
        }

        public static string? GetEmail(string token)
        {
            return ParseToken(token, "email");
        }

        public static string? GetUsername(string token)
        {
            return ParseToken(token, "username");
        }
        private static string? ParseToken(string token, string key)
        {
            try
            {
                // Убираем "Bearer " из токена, если он есть
                if (token.StartsWith("Bearer "))
                {
                    token = token.Substring("Bearer ".Length);
                }

                var handler = new JwtSecurityTokenHandler();

                // Считываем токен
                var jwtToken = handler.ReadJwtToken(token);
                var payload = jwtToken.Payload;

                // Ищем значение клейма
                return payload.Claims.FirstOrDefault(c => c.Type == key)?.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing token: {ex.Message}");
                return null;
            }
        }
    }
}