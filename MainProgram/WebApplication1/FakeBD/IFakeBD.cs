using MainProgram.Model;

namespace WebApplication1.FakeBD
{
    public interface IFakeBD
    {
        public void AddUserOnBD(User user);
        public void AddPostOnBD(Post post);
        public User ReturnUserOnID(User user);
        public Post ReturnPostOnID(Post user);
    }
}
