using MainProgram.Model;

namespace MainProgram.Repositories
{
    public class UsersRepository : IUserRepository
    {
        private DataBase dataBase = new DataBase();

        public async Task AddUserAsync(User user)
        {
            await Task.Run(() => dataBase.Add(user));
        }

    }
}
