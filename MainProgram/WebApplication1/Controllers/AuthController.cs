using MainProgram.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Auth;
using WebApplication1.FakeBD;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        [HttpPost]
        public IActionResult CreateUser(string email, string password, string role)
        {
            User newUser = AuthUser.RegisterNewUser(email, password, role);

            if (newUser == null)
            {
                // нужно вернуть что то  
                return NotFound(); // тут спросить что вернуть ибо ошибка выдается
            }
            else
            {
                BD.AddUserOnBD(newUser);

                return Ok(newUser); // Возвращение нового пользователя в качестве ответа
            }
           
        }
    }
}