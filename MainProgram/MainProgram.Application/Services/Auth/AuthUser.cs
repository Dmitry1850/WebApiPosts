using MainProgram.Model;

namespace MainProgram.Services
{
    public static class AuthUser
    {
        public static User RegisterNewUser(string Email, string Password, string Role)
        {

            ///<summary>
            ///Тут в общем то все проверки 
            /// </summary>

            if (Email == null || Password == null || Role == null) // проверка на null
            {
                return null; // спросить можно ли вернуть налл
            }


            switch (Role.ToLower())      // проверка, верна ли роль
            {
                case "author":
                    break;
                case "reader":
                    break;
                default:
                    return null;
            }

            // Пока нету проверки на правильность емаила, к сожалению

            
            return new User(Email, Password, Role, string.Empty);
        }
    }
}
