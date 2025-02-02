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
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
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

            post.Images.Remove(image);

            await _postRepository.UpdatePost(post);
            return true;
        }

        public async Task<List<Image>> AddImage(Guid postId, string authorId, List<IFormFile> images)
        {
            Console.WriteLine($"[AddImage] Начало метода для поста {postId}");

            var post = await _postRepository.GetPostById(postId);
            if (post == null)
            {
                Console.WriteLine($"[AddImage] Пост {postId} не найден.");
                throw new NotFoundException("Post not found.");
            }

            if (post.AuthorId.ToString() != authorId)
            {
                Console.WriteLine($"[AddImage] Доступ запрещен для пользователя {authorId}");
                throw new Exception("Access denied.");
            }

            Console.WriteLine($"[AddImage] Найден пост {postId}, автор {authorId}, количество картинок: {images.Count}");

            var uploadedImages = new List<Image>();

            if (post.Images == null)
                post.Images = new List<Image>();

            foreach (var image in images)
            {
                if (image.Length == 0) continue;

                var id = Guid.NewGuid();
                var imageUrl = "https://example.com"; 

                var newImage = new Image(id, postId, imageUrl, DateTime.UtcNow);
                post.Images.Add(newImage);
                uploadedImages.Add(newImage);
            }

            Console.WriteLine($"[AddImage] Добавлено {uploadedImages.Count} изображений, пробуем сохранить...");

            bool saved = false;
            int retryCount = 3;

            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    await _postRepository.UpdatePost(post);
                    Console.WriteLine("[AddImage] Успешное обновление поста!");
                    saved = true;
                    break;  
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine($"[AddImage] Конфликт обновления! Попытка {i + 1}. {ex.Message}");

                    post = await _postRepository.GetPostById(postId);
                    if (post == null)
                    {
                        Console.WriteLine($"[AddImage] Пост {postId} не найден при повторном запросе.");
                        throw new NotFoundException("Post not found.");
                    }

                    foreach (var img in uploadedImages)
                        if (!post.Images.Any(i => i.ImageId == img.ImageId))
                            post.Images.Add(img);
                }
            }

            if (!saved)
            {
                Console.WriteLine("[AddImage] Ошибка: запись так и не обновилась после 3 попыток!");
                throw new Exception("Failed to update the record after multiple attempts.");
            }

            Console.WriteLine($"[AddImage] Метод завершён, возвращаем {uploadedImages.Count} изображений.");
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

        public async Task<bool> DeletePost(Guid postId, string authorId)
        {
            var post = await _postRepository.GetPostById(postId);
            if (post == null)
                throw new NotFoundException("Post not found.");
            if (post.AuthorId.ToString() != authorId)
                throw new Exception("Access denied.");

            await _postRepository.DeletePost(postId);
            return true;
        }
    }
}
