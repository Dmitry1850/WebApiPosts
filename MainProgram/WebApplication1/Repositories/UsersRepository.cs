using MainProgram.Model;

namespace MainProgram.Repositories
{
    public class UsersRepository : IUserRepository
    {
        private List<User> usersBase = new List<User>();

        public async Task<User> GetUser(string Email)
        {
            for (int i = 0; i < usersBase.Count; i++)
            {
                if (usersBase[i].Email == Email)
                    return await Task.Run(() => (usersBase[i]));
            }

            return null;
        }
        public async Task<List<User>> AddUser(User user)
        {
            usersBase.Add(user);

            return await Task.Run(() => (usersBase));
        }
        public async Task<bool> UserExists(string email)
        {
            return await Task.Run(() => (usersBase.Any(u => u.Email == email)));
        }

        public async Task<List<User>> ReturnAll()
        {
            return await Task.Run(() => (usersBase));
        }
    }
}
