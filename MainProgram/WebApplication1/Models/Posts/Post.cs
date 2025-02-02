using System.ComponentModel.DataAnnotations;

namespace MainProgram.Model
{
    public class Post
    {
        [Key]
        public Guid PostId { get; set; } = Guid.NewGuid();
        public Guid AuthorId { get; set; }
        public string? IdempotencyKey { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Draft";
        public List<Image> Images { get; set; } = new();
    }
}