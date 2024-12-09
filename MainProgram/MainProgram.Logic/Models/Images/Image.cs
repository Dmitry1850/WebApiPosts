namespace MainProgram.Model
{
    public class Image
    {
        public Guid imageId { get; set; }
        public Guid postId { get; set; }
        public string imageUrl { get; set; }
        public DateTime createdAt { get; set; }
    }
}
