using MainProgram.Model;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContextPosts : DbContext
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<Image> Images { get; set; }

    public ApplicationDbContextPosts(DbContextOptions<ApplicationDbContextPosts> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Post>()
            .HasMany(p => p.Images)
            .WithOne()
            .HasForeignKey(i => i.PostId);
    }
}