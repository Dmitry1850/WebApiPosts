﻿using MainProgram.Model;

namespace MainProgram.Repositories
{
    public interface IUserRepository
    {
        Task<User> ReturnUser(string Email);
        Task AddUser(User user);
    }
}
