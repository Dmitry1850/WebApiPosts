using MainProgram.Interfaces;

namespace MainProgram.Auth
{
    public class AuthSettings(IConfiguration configuration) : IAuthSettings
    {
        public string Issuer => configuration["Auth:Issuer"] ?? string.Empty;
        public string Audience => configuration["Auth:Audience"] ?? string.Empty;
        public string Key => configuration["Auth:Key"] ?? string.Empty;
        public int TokenExpiresAfterHours => int.Parse(configuration["Auth:TokenExpiresAfterHours"] ?? string.Empty);
    }
}
