using MainProgram.Model;

namespace WebApplication1.FakeBD
{
    public static class BD //: IFakeBD
    {
        private static List<User> users = new List<User>();
        private static List<Post> posts = new List<Post>();

        public static void AddUserOnBD(User user)
        {
            users.Add(user);
        }
        public static void AddPostOnBD(Post post)
        { 
            posts.Add(post);
        }

        public static User ReturnUserOnID(User user)
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
        public static Post ReturnPostOnID(Post post)
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
