using MainProgram.Model;

namespace MainProgram.Repositories
{
    public class PostsRepository : IPostRepository
    {
        private static readonly List<Post> _posts = new();

        public async Task<Post?> GetPostById(Guid postId)
        {
            return _posts.FirstOrDefault(p => p.PostId == postId);
        }

        public async Task<List<Post>> GetPostsByAuthorId(Guid authorId)
        {
            return _posts.Where(p => p.AuthorId == authorId).ToList();
        }

        public async Task<List<Post>> GetPublishedPosts()
        {
            return _posts.Where(p => p.Status == "Published").ToList();
        }

        public async Task AddPost(Post post)
        {
            _posts.Add(post);
        }

        public async Task UpdatePost(Post post)
        {
            var existingPost = _posts.FirstOrDefault(p => p.PostId == post.PostId);
            if (existingPost != null)
            {
                _posts.Remove(existingPost);
                _posts.Add(post);
            }
        }

        public async Task DeletePost(Guid postId)
        {
            var post = _posts.FirstOrDefault(p => p.PostId == postId);
            if (post != null)
            {
                _posts.Remove(post);
            }
        }
    }
}