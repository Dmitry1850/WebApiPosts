using MainProgram.Model;

namespace MainProgram.Repositories
{
    public class UsersRepository : IUserRepository
    {
        private DataBase dataBase = new DataBase();

        public async Task<User> ReturnUserOnGuidAsync(Guid guid)
        {
            return await Task.Run(() => dataBase.ReturnUserOnGuid(guid));
        }
        public async Task AddUserOnBaseAsync(User user)
        {
            await Task.Run(() => dataBase.AddUserOnBase(user));
        }
    }
}
