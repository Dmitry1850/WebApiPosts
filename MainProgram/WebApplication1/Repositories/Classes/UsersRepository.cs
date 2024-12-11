using MainProgram.Model;
using System.Collections.Generic;

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
        public async Task AddUser(User user)
        {
            await Task.Run(() => usersBase.Add(user));
        }
        public bool UserExists(string email)
        {
            return usersBase.Any(u => u.Email == email);
        }

        public List<User> ReturnAll()
        {
            return usersBase;
        }
    }
}
