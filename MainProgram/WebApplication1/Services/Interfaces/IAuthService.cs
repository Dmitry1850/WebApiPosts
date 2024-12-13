using Microsoft.AspNetCore.Identity.Data;
using MainProgram.AllRequests;
using MainProgram.Auth;

namespace MainProgram.Interfaces;

public interface IAuthService
{
    Task<AuthResponse?> Register(RegisterModel registerModel);
    Task<AuthResponse?> Login(LoginModel loginModel);
}