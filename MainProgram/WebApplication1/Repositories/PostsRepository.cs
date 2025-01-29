using Microsoft.EntityFrameworkCore;
using MainProgram.Model;
using MainProgram.Data;
using MainProgram.Exceptions;

namespace MainProgram.Repositories
{
    public class PostsRepository : IPostRepository
    {
        private readonly ApplicationDbContextPosts _context;

        public PostsRepository(ApplicationDbContextPosts context)
        {
            _context = context;
        }

        public async Task<Post?> GetPostById(Guid postId)
        {
            return await _context.Posts.FirstOrDefaultAsync(p => p.PostId == postId);
        }

        public async Task<List<Post>> GetPostsByAuthorId(Guid authorId)
        {
            return await _context.Posts.Where(p => p.AuthorId == authorId).ToListAsync();
        }

        public async Task<List<Post>> GetPublishedPosts()
        {
            return await _context.Posts.Where(p => p.Status == "Published").ToListAsync();
        }

        public async Task AddPost(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePost(Post post)
        {
            var existingPost = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == post.PostId);

            if (existingPost != null)
            {
                _context.Entry(existingPost).CurrentValues.SetValues(post);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException("Post not found.");
            }

        }

        public async Task DeletePost(Guid postId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == postId);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }
    }

}