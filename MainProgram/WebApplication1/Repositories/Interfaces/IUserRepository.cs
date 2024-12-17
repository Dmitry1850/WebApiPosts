using MainProgram.Model;

namespace MainProgram.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUser(string Email);
        Task<List<User>> AddUser(User user);
        Task<bool> UserExists(string email);
        Task<List<User>> ReturnAll();
    }
}
