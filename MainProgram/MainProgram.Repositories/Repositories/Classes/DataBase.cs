using MainProgram.Model;

namespace MainProgram.Repositories
{
    public class DataBase
    {
        private List<User> users = new List<User>();

        public User ReturnUserOnGuid(Guid guid)
        {
            for (int i = 0; i < users.Count; i++)
            { 
                return users[i];
            }
            return null;
        }
        public void AddUserOnBase(User newUser)
        {
            #region Пока тест
            //пока тест
            users.Add(newUser);
            #endregion
        }
    }
}
