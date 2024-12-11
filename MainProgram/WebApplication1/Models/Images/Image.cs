namespace MainProgram.Model
{
    public class Image
    {
        public Image(Guid ImageID, Guid PostID, string ImageURL, DateTime CreatedAt)
        { 
            imageId = ImageID;
            postId = PostID;
            imageUrl = ImageURL;
            createdAt = CreatedAt;
        }

        public Guid imageId { get; set; }
        public Guid postId { get; set; }
        public string imageUrl { get; set; }
        public DateTime createdAt { get; set; }
    }
}
