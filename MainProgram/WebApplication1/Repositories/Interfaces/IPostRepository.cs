using MainProgram.Model;

namespace MainProgram.Repositories
{
    public interface IPostRepository
    {
        Task<Post> ReturnPost(Guid PostID);
        Task AddPost(Post post);
        Task DeletePost(Guid PostID);
    }
}