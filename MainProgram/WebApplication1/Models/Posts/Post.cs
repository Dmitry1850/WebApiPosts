using System.ComponentModel.DataAnnotations;

namespace MainProgram.Model
{
    public class Post
    {
        public Post(Guid postID, Guid authorID, string idempotencyKey, string title, string content, DateTime createdAt, DateTime updatedAt, string status)
        {
            PostId = Guid.NewGuid();
            AuthorId = authorID;
            IdempotencyKey = idempotencyKey;
            Title = title;
            Content = content;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            Status = status;
            Images = new List<Image>();
        }

        public Post() { }

        public Guid PostId { get; set; }
        public Guid AuthorId { get; set; }
        public string IdempotencyKey { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Status { get; set; } // Draft or Published
        public List<Image> Images { get; set; }

        //[Timestamp] 
        //public byte[] RowVersion { get; set; }
    }
}