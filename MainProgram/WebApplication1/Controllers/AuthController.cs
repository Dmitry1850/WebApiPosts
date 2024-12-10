using MainProgram.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MainProgram.Services;

namespace MainProgram.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        [HttpPost]
        //public IActionResult CreateUser(string email, string password, string role)
        //{
        //    User newUser = AuthService.RegisterNewUser(email, password, role);

        //    if (newUser == null)
        //    {
        //        // нужно вернуть что то  
        //        return NotFound(); // тут спросить что вернуть ибо ошибка выдается
        //    }
        //    else
        //    {

        //        return Ok(newUser); // Возвращение нового пользователя в качестве ответа
        //    }
           
        //}
    }
}