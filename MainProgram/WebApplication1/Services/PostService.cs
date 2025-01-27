using MainProgram.AllRequests;
using MainProgram.Exceptions;
using MainProgram.Interfaces;
using MainProgram.Repositories;
using MainProgram.Model;
using Microsoft.EntityFrameworkCore;

namespace MainProgram.Auth
{
    public class PostService : IPostCervice
    {
        private IPostRepository _postRepository;
        private readonly ApplicationDbContextPosts _context; // Добавляем контекст данных

        public PostService(IPostRepository postRepository, ApplicationDbContextPosts context)
        {
            _postRepository = postRepository;
            _context = context; // Инициализация контекста
        }

        public async Task<Post?> GetPostById(Guid id)
        {
            return await _postRepository.GetPostById(id);
        }

        public async Task<List<Post>> GetPostsByAuthorId(Guid authorId)
        {
            return await _postRepository.GetPostsByAuthorId(authorId);
        }

        public async Task<List<Post>> GetPublishedPosts()
        {
            return await _postRepository.GetPublishedPosts();
        }

        public async Task<Post> CreatePost(string authorId, CreatePostRequest postRequest)
        {
            Post newPost = new Post(Guid.NewGuid(), Guid.Parse(authorId), postRequest.IdempotencyKey, postRequest.Title, postRequest.Content, DateTime.UtcNow, DateTime.UtcNow, "Draft");

            await _postRepository.AddPost(newPost);

            return newPost;
        }

        public async Task<Post?> UpdatePost(Guid postId, string authorId, UpdatePost updatePost)
        {
            var post = await _postRepository.GetPostById(postId);
            if (post == null)
                throw new NotFoundException("Post not found.");
            if (post.AuthorId.ToString() != authorId)
                throw new Exception("Access denied.");

            post.Title = updatePost.Title;
            post.Content = updatePost.Content;
            post.UpdatedAt = DateTime.UtcNow;

            await _postRepository.UpdatePost(post);

            return post;
        }

        public async Task<bool> DeleteImage(Guid postId, Guid imageId, string authorId)
        {
            var post = await _postRepository.GetPostById(postId);

            if (post == null)
                throw new NotFoundException("Post not found.");
            if (post.AuthorId.ToString() != authorId)
                throw new Exception("Access denied.");

            var image = post.Images.FirstOrDefault(img => img.ImageId == imageId);
            if (image == null)
                throw new NotFoundException("Image not found.");

            var objectName = $"{postId}/{imageId}";

            post.Images.Remove(image);

            await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных

            return true;
        }

        public async Task<List<Image>> AddImage(Guid postId, string authorId, List<IFormFile> images)
        {
            var post = await _postRepository.GetPostById(postId);
            if (post == null)
                throw new NotFoundException("Post not found.");

            if (post.AuthorId.ToString() != authorId)
                throw new Exception("Access denied.");

            if (post.Images == null)
            {
                post.Images = new List<Image>();
            }

            var uploadedImages = new List<Image>();

            foreach (var image in images)
            {
                if (image.Length == 0) continue;

                var id = Guid.NewGuid();
                var objectName = $"{postId}/{id}";

                await using var stream = image.OpenReadStream();

                string imageUrl = "https://example.com/image.jpg";

                var newImage = new Image(id, postId, imageUrl, DateTime.UtcNow);

                post.Images.Add(newImage);
                uploadedImages.Add(newImage);
            }

            await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных

            return uploadedImages;
        }

        public async Task<Post?> PublishPost(Guid postId, string authorId, PublishPostRequest request)
        {
            var post = await _postRepository.GetPostById(postId);
            if (post == null)
                throw new NotFoundException("Post not found.");
            if (post.AuthorId.ToString() != authorId)
                throw new Exception("Access denied.");

            post.Status = request.Status;
            post.UpdatedAt = DateTime.UtcNow;

            await _postRepository.UpdatePost(post);

            return post;
        }
    }
}
