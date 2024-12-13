using MainProgram.Model;

namespace MainProgram.Repositories
{
    public class PostsRepository : IPostRepository
    {
        private static readonly List<Post> _posts = new();

        public async Task<Post?> GetPostById(Guid postId)
        {
            return _posts.FirstOrDefault(p => p.postId == postId);
        }

        public async Task<List<Post>> GetPostsByAuthorId(Guid authorId)
        {
            return _posts.Where(p => p.authorId == authorId).ToList();
        }

        public async Task<List<Post>> GetPublishedPosts()
        {
            return _posts.Where(p => p.status == "Published").ToList();
        }

        public async Task AddPost(Post post)
        {
            _posts.Add(post);
        }

        public async Task UpdatePost(Post post)
        {
            var existingPost = _posts.FirstOrDefault(p => p.postId == post.postId);
            if (existingPost != null)
            {
                _posts.Remove(existingPost);
                _posts.Add(post);
            }
        }

        public async Task DeletePost(Guid postId)
        {
            var post = _posts.FirstOrDefault(p => p.postId == postId);
            if (post != null)
            {
                _posts.Remove(post);
            }
        }
    }
}
