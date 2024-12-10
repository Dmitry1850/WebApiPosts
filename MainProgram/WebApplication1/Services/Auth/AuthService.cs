﻿using MainProgram.API.Services.Interfaces;
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

namespace MainProgram.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        /// <summary>
        /// Регистрация нового пользователя.
        /// </summary>
        public (string AccessToken, string RefreshToken) Register(RegisterRequest request)
        {
            ValidateRegisterRequest(request);

            if (_userRepository.UserExists(request.Email))
            {
                throw new BadRequestException("Email already registered.");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new User
            {
                userId = Guid.NewGuid(),
                email = request.Email,
                passwordHash = hashedPassword,
                role = request.Role,
                refreshToken = string.Empty,
                refreshTokenExpiryTime = DateTime.MinValue
            };

            var accessToken = GenerateJsonWebToken(newUser, TimeSpan.FromHours(2));
            var refreshToken = GenerateRefreshToken();

            newUser.RefreshToken = refreshToken.Token;
            newUser.RefreshTokenExpiryTime = refreshToken.Expiry;

            _userRepository.AddUser(newUser);

            return (accessToken, refreshToken.Token);
        }

        /// <summary>
        /// Аутентификация пользователя.
        /// </summary>
        public (string AccessToken, string RefreshToken) Login(LoginRequest request)
        {
            var user = _userRepository.GetUserByEmail(request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new NotFoundException("Invalid email or password.");
            }

            var accessToken = GenerateJsonWebToken(user, TimeSpan.FromHours(2));
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken.Token;
            user.RefreshTokenExpiryTime = refreshToken.Expiry;

            return (accessToken, refreshToken.Token);
        }

        /// <summary>
        /// Обновление токена.
        /// </summary>
        public (string AccessToken, string RefreshToken) RefreshToken(RefreshTokenRequest request)
        {
            var user = _userRepository.ReturnAll().FirstOrDefault(u => u.RefreshToken == request.RefreshToken);

            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                throw new BadRequestException("Refresh Token is invalid.");
            }

            var newAccessToken = GenerateJsonWebToken(user, TimeSpan.FromHours(2));
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken.Token;
            user.RefreshTokenExpiryTime = newRefreshToken.Expiry;

            return (newAccessToken, newRefreshToken.Token);
        }

        private void ValidateRegisterRequest(RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                string.IsNullOrWhiteSpace(request.Role))
            {
                throw new BadRequestException("All fields are required.");
            }

            if (!Regex.IsMatch(request.Email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
            {
                throw new BadRequestException("Invalid email format.");
            }

            if (request.Role != "Author" && request.Role != "Reader")
            {
                throw new BadRequestException("Invalid role.");
            }
        }

        private string GenerateJsonWebToken(User user, TimeSpan expiryDuration)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtOptions:SigningKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                _config["JwtOptions:Issuer"],
                _config["JwtOptions:Audience"],
                claims,
                expires: DateTime.UtcNow.Add(expiryDuration),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private (string Token, DateTime Expiry) GenerateRefreshToken()
        {
            var refreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            return (refreshToken, DateTime.UtcNow.AddDays(7));
        }
    }
}