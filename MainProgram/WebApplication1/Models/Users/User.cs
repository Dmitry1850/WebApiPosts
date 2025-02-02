namespace MainProgram.Model
{
    public class User
    {
        public User(Guid userId, string email, string passwordHash, int role, string refreshToken, DateTime refreshTokenExpiryTime)
        {
            UserId = userId;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            RefreshToken = refreshToken;
            RefreshTokenExpiryTime = refreshTokenExpiryTime;
        }

        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int Role { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}