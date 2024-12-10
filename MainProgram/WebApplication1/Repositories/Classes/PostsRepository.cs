using MainProgram.Model;

namespace MainProgram.Repositories
{
    public class PostsRepository : IPostRepository
    {
        private List<Post> posts = new List<Post>();

        public async Task AddPost(Post post)
        {
            posts.Add(post);
        }

        public async Task DeletePost(Guid PostID)
        {
            for (int i = 0; i < posts.Count; i++)
            {
                if (PostID == posts[i].postId)
                    await Task.Run(() => (posts.Remove(posts[i])));
            }
        }

        public async Task<Post> ReturnPost(Guid PostID)
        {
            for (int i = 0; i < posts.Count; i++)
            {
                if (PostID == posts[i].postId)
                    return await Task.Run(() => (posts[i]));
            }

            return null;
        }
    }
}
