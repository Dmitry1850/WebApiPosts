using Microsoft.EntityFrameworkCore;
using MainProgram.Model;

namespace MainProgram.Data
{
    public class ApplicationDbContextUsers : DbContext
    {
        public ApplicationDbContextUsers(DbContextOptions<ApplicationDbContextUsers> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }
    }
}
