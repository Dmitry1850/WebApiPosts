using MainProgram.AllRequests;
using MainProgram.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MainProgram.Interfaces
{
    public interface IPostCervice
    {
        Task<Post?> GetPostById(Guid id);
        Task<List<Post>> GetPostsByAuthorId(Guid authorId);
        Task<List<Post>> GetPublishedPosts();
        Task<Post> CreatePost(string authorId, CreatePostRequest postRequest);
        Task<Post?> UpdatePost(Guid postId, string authorId, UpdatePost updatePost);
        Task<bool> DeleteImage(Guid postId, Guid imageId, string authorId);
        Task<List<Image>> AddImage(Guid postId, string authorId, List<IFormFile> images);
        Task<Post?> PublishPost(Guid postId, string authorId, PublishPostRequest request);
    }
}
