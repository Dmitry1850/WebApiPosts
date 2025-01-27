namespace MainProgram.Model
{
    public class Image
    {
        public Image() { }

        public Image(Guid imageID, Guid postID, string imageURL, DateTime createdAt)
        {
            ImageId = imageID;
            PostId = postID;
            ImageUrl = imageURL;
            CreatedAt = createdAt;
        }

        public Guid ImageId { get; set; }
        public Guid PostId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
