using MainProgram.Model;

namespace MainProgram.Repositories
{
    public interface IPostRepository
    {
        Task<Post?> GetPostById(Guid postId);
        Task<List<Post>> GetPostsByAuthorId(Guid authorId);
        Task<List<Post>> GetPublishedPosts();
        Task AddPost(Post post);
        Task UpdatePost(Post post);
        Task DeletePost(Guid postId);
    }
}