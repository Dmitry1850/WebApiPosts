using System.ComponentModel.DataAnnotations;

namespace MainProgram.Model
{
    public class Image
    {
        [Key]
        public Guid ImageId { get; set; } = Guid.NewGuid();
        public Guid PostId { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}