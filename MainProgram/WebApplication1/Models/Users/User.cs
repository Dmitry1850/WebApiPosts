using MainProgram.Model;

namespace MainProgram.Model
{
    public class User 
    {
        public Guid userId { get; set; }
        public string email { get; set; }
        public string passwordHash { get; set; }
        public string role { get; set; }
        public string refreshToken { get; set; }
        public DateTime refreshTokenExpiryTime { get; set; }
    }
}
