using Microsoft.EntityFrameworkCore;
using MainProgram.Model;

namespace MainProgram.Data
{
    public class ApplicationDbContextPosts : DbContext
    {
        public ApplicationDbContextPosts(DbContextOptions<ApplicationDbContextPosts> options)
            : base(options)
        { }

        public DbSet<Post> Posts { get; set; }
    }
}