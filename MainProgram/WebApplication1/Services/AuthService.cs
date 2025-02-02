using MainProgram.Interfaces;
using MainProgram.Common;
using MainProgram.Model;
using MainProgram.Repositories;
using System.Security.Claims;
using MainProgram.Auth;
using MainProgram.Users;

namespace MainProgram.Services
{
    public class AuthService(IUserRepository userRepository, ITokenService tokenService, IAuthSettings authSettings) : IAuthService
    {
        public async Task<AuthResponse?> Register(RegisterModel registerModel)
        {
            var user = await userRepository.GetUser(registerModel.Username) ?? await userRepository.GetUser(registerModel.Email);
            if (user != null)
                return null;

            var refreshToken = await tokenService.CreateToken(new List<Claim>());
            User newUser = new User(Guid.NewGuid(), registerModel.Email, await Hash.GetHash(registerModel.Password), (int)Role.Author, refreshToken, DateTime.UtcNow.AddHours(authSettings.TokenExpiresAfterHours));
            var id = await userRepository.AddUser(newUser);

            var claims = tokenService.GetClaims(newUser.UserId, (int)Role.Author, registerModel.Email);
            var accessToken = await tokenService.CreateToken(claims, 24);

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthResponse?> Login(LoginModel loginModel)
        {
            var user = await userRepository.GetUser(loginModel.Login);

            if (user == null || user.PasswordHash != await Hash.GetHash(loginModel.Password))
                return null;

            var claims = tokenService.GetClaims(user.UserId, user.Role, user.Email); 
            var accessToken = await tokenService.CreateToken(claims, 24);

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = user.RefreshToken,
            };
        }
    }
}