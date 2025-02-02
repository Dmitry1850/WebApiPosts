using MainProgram.AllRequests;
using MainProgram.Exceptions;
using MainProgram.Interfaces;
using MainProgram.Repositories;
using MainProgram.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Minio.Exceptions;

namespace MainProgram.Auth
{
    public class PostService : IPostCervice
    {
        private readonly IPostRepository _postRepository;
        private readonly IMinioRepository _minioRepository;

        public PostService(IPostRepository postRepository, IMinioRepository minioRepository)
        {
            _postRepository = postRepository;
            _minioRepository = minioRepository;
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
            var newPost = new Post
            {
                PostId = Guid.NewGuid(),
                AuthorId = Guid.Parse(authorId),
                IdempotencyKey = postRequest.IdempotencyKey,
                Title = postRequest.Title,
                Content = postRequest.Content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Status = "Draft"
            };

            // Сохраняем новый пост в базе данных
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
            var post = await _postRepository.GetPostById(postId);
            if (post == null)
                throw new NotFoundException("Post not found.");
            if (post.AuthorId.ToString() != authorId)
                throw new ForbiddenException("Access denied.");

            var bucketName = "post-images";
            await _minioRepository.CreateBucketAsync(bucketName);

            var uploadedImages = new List<Image>();

            foreach (var image in images)
            {
                if (image.Length == 0) continue;

                var id = Guid.NewGuid();
                var objectName = $"{postId}/{id}";

                await using var stream = image.OpenReadStream();
                await _minioRepository.UploadObjectAsync(bucketName, objectName, stream, image.Length, image.ContentType);

                var imageUrl = await _minioRepository.GetPresignedUrlAsync(bucketName, objectName, 3600);

                var newImage = new Image
                {
                    ImageId = id,
                    PostId = postId,
                    ImageUrl = imageUrl,
                    CreatedAt = DateTime.UtcNow
                };

                await _postRepository.AddImage(newImage);
                uploadedImages.Add(newImage);
            }
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
