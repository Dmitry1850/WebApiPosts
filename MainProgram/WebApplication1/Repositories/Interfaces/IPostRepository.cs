using MainProgram.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MainProgram.Repositories
{
    public interface IPostRepository
    {
        Task<Post?> GetPostById(Guid postId);
        Task<List<Post>> GetPostsByAuthorId(Guid authorId);
        Task<List<Post>> GetPublishedPosts();
        Task AddImage(Image image);
        Task AddPost(Post post);
        Task UpdatePost(Post post);
        Task DeletePost(Guid postId);
    }
}