namespace MainProgram.Model
{
    public class Post
    {
        public Post(Guid PostID, Guid AuthorID, string IdempotencyKey, string Title, string Content, DateTime CreatedAt, DateTime UpdatedAt, string Status) 
        { 
            postId = Guid.NewGuid();
            authorId = AuthorID;
            idempotencyKey = IdempotencyKey;
            title = Title;
            content = Content;
            createdAt = CreatedAt;
            updatedAt = UpdatedAt;
            status = Status;
        }

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
