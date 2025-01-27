using Microsoft.EntityFrameworkCore;
using MainProgram.Model;
using MainProgram.Data;

namespace MainProgram.Repositories
{
    public class UsersRepository : IUserRepository
    {
        private readonly ApplicationDbContextUsers _context;

        public UsersRepository(ApplicationDbContextUsers context)
        {
            _context = context;
        }

        public async Task<User?> GetUser(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<List<User>> AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> UserExists(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<List<User>> ReturnAll()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
