using MainProgram.Interfaces;
using MainProgram.Common;
using MainProgram.Exceptions;
using MainProgram.Model;
using MainProgram.Repositories;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Text;
using MainProgram.AllRequests;
using MainProgram.Auth;
using MainProgram.Interfeices;
using MainProgram.Users;

namespace MainProgram.Services
{
    public class AuthService(IUserRepository userRepository, ITokenService tokenService, IAuthSettings authSettings) : IAuthService
    {
        public async Task<AuthResponse?> Register(RegisterModel registerModel)
        {
            var user = await userRepository.GetUser(registerModel.Username) ?? await userRepository.GetUser(registerModel.Email);
            if (user != null)
            {
                return null;
            }

            var refreshToken = tokenService.CreateToken(new List<Claim>());
            User newUser = new User(Guid.NewGuid(), registerModel.Email, Hash.GetHash(registerModel.Password), (int)Role.User, refreshToken, DateTime.UtcNow.AddHours(authSettings.TokenExpiresAfterHours));
            var id = userRepository.AddUser(newUser);

            var claims = Jwt.GetClaims(newUser.UserId, (int)Role.User, registerModel.Email);
            var accessToken = tokenService.CreateToken(claims, 24);

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthResponse?> Login(LoginModel loginModel)
        {
            var user = await userRepository.GetUser(loginModel.Login);
            if (user == null || user.PasswordHash != Hash.GetHash(loginModel.Password))
            {
                return null;
            }

            var claims = Jwt.GetClaims(user.UserId, user.Role, user.Email);
            var accessToken = tokenService.CreateToken(claims, 24);

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = user.RefreshToken,
            };
        }
    }
}