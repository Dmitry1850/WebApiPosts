using MainProgram.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MainProgram.Services;
using MainProgram.Auth;
using MainProgram.Interfaces;

namespace MainProgram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var response = await authService.Login(loginModel);
            if (response == null)
            {
                return BadRequest();
            }

            return Ok(response);
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            var response = await authService.Register(registerModel);
            if (response == null)
            {
                return BadRequest();
            }

            return Ok(response);
        }
    }
}