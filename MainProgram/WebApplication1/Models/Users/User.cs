using MainProgram.Model;

namespace MainProgram.Model
{
    public class User 
    {
        public User(string Email, string PasswordHash, string Role, string RefreshToken)
        { 
            userId = Guid.NewGuid();
            email = Email;
            passwordHash = PasswordHash;
            role = Role;
            refreshToken = RefreshToken;
            refreshTokenExpiryTime = DateTime.Now;
        }
        public Guid userId { get; set; }
        public string email { get; set; }
        public string passwordHash { get; set; }
        public string role { get; set; } // author or reader Все в НИЖНЕМ регистре!!!!!!!!!!
        public string refreshToken { get; set; }
        public DateTime refreshTokenExpiryTime { get; set; }
    }
}
