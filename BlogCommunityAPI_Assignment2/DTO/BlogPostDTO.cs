namespace BlogCommunityAPI_Assignment2.DTO
{
    public class BlogPostDTO
    {
        public int? PostID { get; set; }  // Gör det nullable
        public string Title { get; set; }
        public string Content { get; set; }
        public int CategoryID { get; set; }
        public int? UserID { get; set; }
    }
}
