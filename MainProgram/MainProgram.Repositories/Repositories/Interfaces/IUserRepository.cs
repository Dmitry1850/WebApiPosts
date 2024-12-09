using MainProgram.Model;

namespace MainProgram.Repositories
{
    public interface IUserRepository
    {
        Task<User> ReturnUserOnGuidAsync(Guid guid);
        Task AddUserOnBaseAsync(User user);
    }
}
