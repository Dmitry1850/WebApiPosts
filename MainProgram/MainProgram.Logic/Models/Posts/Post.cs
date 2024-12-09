using MainProgram.Model;

namespace MainProgram.Model
{
    public class Post
    {
        public Guid postId { get; set; }
        public Guid authorId { get; set; }
        public string idempotencyKey { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string status { get; set; } // Draft or Published
        public List<Image> images { get; set; }
    }
}
