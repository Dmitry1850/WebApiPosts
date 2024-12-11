using MainProgram.AllRequests;
using MainProgram.Exceptions;
using MainProgram.Interfaces;
using MainProgram.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MainProgram.Model;

namespace MainProgram.Auth
{
    public class PostService : IPostService
    {
        //private readonly IMinioRepository _minioRepository;
        private readonly IPostRepository _postRepository;
        //private readonly IIdempotencyKeysRepository _usedIdempotencyKeys;

        public PostService(IPostRepository postRepository)
        {
            //_minioRepository = minioRepository;
            _postRepository = postRepository;
            //_usedIdempotencyKeys = idempotencyKeysRepository;
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
            //if (_usedIdempotencyKeys.Contains(postRequest.IdempotencyKey))
            //    throw new ConflictException("IdempotencyKey has already been used.");

            Post newPost = new Post(Guid.NewGuid(), Guid.Parse(authorId), postRequest.IdempotencyKey, postRequest.Title, postRequest.Content, DateTime.UtcNow, DateTime.UtcNow, "Draft");
            
            await _postRepository.AddPost(newPost);
            //_usedIdempotencyKeys.Add(postRequest.IdempotencyKey);

            return newPost;
        }

        public async Task<Post?> UpdatePost(Guid postId, string authorId, UpdatePost updatePost)
        {
            var post = await _postRepository.GetPostById(postId);
            if (post == null)
                throw new NotFoundException("Post not found.");
            if (post.authorId.ToString() != authorId)
                throw new Exception("Access denied.");

            post.title = updatePost.Title;
            post.content = updatePost.Content;
            post.updatedAt = DateTime.UtcNow;

            await _postRepository.UpdatePost(post);

            return post;
        }


        public async Task<bool> DeleteImage(Guid postId, Guid imageId, string authorId)
        {
            var post = await _postRepository.GetPostById(postId);

            if (post == null)
                throw new NotFoundException("Post not found.");
            if (post.authorId.ToString() != authorId)
                throw new Exception("Access denied.");

            var image = post.images.FirstOrDefault(img => img.imageId == imageId);
            if (image == null)
                throw new NotFoundException("Image not found.");

            var bucketName = "post-images";
            var objectName = $"{postId}/{imageId}";
            //await _minioRepository.DeleteObjectAsync(bucketName, objectName);

            post.images.Remove(image);

            return true;
        }

        public async Task<List<Post>> AddImage(Guid postId, string authorId, List<IFormFile> images)
        {
            var post = await _postRepository.GetPostById(postId);
            if (post == null)
                throw new NotFoundException("Post not found.");
            if (post.authorId.ToString() != authorId)
                throw new Exception("Access denied.");

            var bucketName = "post-images";
            //await _minioRepository.CreateBucketAsync(bucketName);

            var uploadedImages = new List<Image>();

            foreach (var image in images)
            {
                if (image.Length == 0) continue;

                var id = Guid.NewGuid();
                var objectName = $"{postId}/{id}";

                await using var stream = image.OpenReadStream();
                //await _minioRepository.UploadObjectAsync(bucketName, objectName, stream, image.Length, image.ContentType);

                //var imageUrl = await _minioRepository.GetPresignedUrlAsync(bucketName, objectName, 3600);

                var newImage = new Image(id, postId, imageUrl, DateTime.UtcNow);

                post.images.Add(newImage);
                uploadedImages.Add(newImage);
            }

            return uploadedImages;
        }

        public async Task<Post?> PublishPost(Guid postId, string authorId, PublishPostRequest request)
        {
            var post = await _postRepository.GetPostById(postId);
            if (post == null)
                throw new NotFoundException("Post not found.");
            if (post.authorId.ToString() != authorId)
                throw new Exception("Access denied.");

            post.status = request.Status;
            post.updatedAt = DateTime.UtcNow;

            await _postRepository.UpdatePost(post);

            return post;
        }

    }
}
