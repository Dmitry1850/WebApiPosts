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
            return await _context.Posts
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.PostId == postId);
        }

        public async Task<List<Post>> GetPostsByAuthorId(Guid authorId)
        {
            return await _context.Posts
                .Include(p => p.Images)
                .Where(p => p.AuthorId == authorId)
                .ToListAsync();
        }

        public async Task<List<Post>> GetPublishedPosts()
        {
            return await _context.Posts
                .Include(p => p.Images)
                .Where(p => p.Status == "Published")
                .ToListAsync();
        }

        public async Task AddPost(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePost(Post post)
        {
            var existingPost = await _context.Posts
                                              .FirstOrDefaultAsync(p => p.PostId == post.PostId);

            if (existingPost == null)
            {
                throw new NotFoundException("Post not found.");
            }

            // Проверка RowVersion на конфликт
            if (!existingPost.RowVersion.SequenceEqual(post.RowVersion))
            {
                throw new DbUpdateConcurrencyException("The record was updated by another user.");
            }

            // Обновление данных поста
            existingPost.Title = post.Title;
            existingPost.Content = post.Content;
            existingPost.UpdatedAt = DateTime.UtcNow;

            // Обновляем RowVersion
            existingPost.RowVersion = post.RowVersion;

            bool saved = false;
            int retryCount = 3;

            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    await _context.SaveChangesAsync();
                    saved = true;
                    break;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine($"[UpdatePost] Конфликт обновления! Попытка {i + 1}. {ex.Message}");

                    // Повторно загружаем актуальные данные и проверяем RowVersion
                    existingPost = await _context.Posts
                                                  .FirstOrDefaultAsync(p => p.PostId == post.PostId);
                    if (existingPost == null)
                    {
                        throw new NotFoundException("Post not found during conflict resolution.");
                    }

                    // Проверяем RowVersion для повторного конфликта
                    if (!existingPost.RowVersion.SequenceEqual(post.RowVersion))
                    {
                        throw new DbUpdateConcurrencyException("The record was updated by another user.");
                    }

                    // Обновляем RowVersion и пробуем сохранить снова
                    existingPost.RowVersion = post.RowVersion;
                }
            }

            if (!saved)
            {
                throw new Exception("Failed to save the post after multiple attempts due to concurrency conflict.");
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
