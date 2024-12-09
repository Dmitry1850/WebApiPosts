using MainProgram.Model;

namespace WebApplication1.FakeBD
{
    public class BD : IFakeBD
    {
        private List<User> users = new List<User>();
        private List<Post> posts = new List<Post>();

        public void AddUserOnBD(User user)
        {
            users.Add(user);
        }
        public void AddPostOnBD(Post post)
        { 
            posts.Add(post);
        }

        public User ReturnUserOnID(User user)
        {
            for (int i = 0; i < users.Count; i++)
            {
                if (user.userId == users[i].userId)
                { 
                    return users[i];
                }
            }

            return null;
        }
        public Post ReturnPostOnID(Post post)
        {
            for (int i = 0; i < posts.Count; i++)
            {
                if (post.postId == posts[i].postId)
                {
                    return posts[i];
                }
            }

            return null;
        }
    }
}
