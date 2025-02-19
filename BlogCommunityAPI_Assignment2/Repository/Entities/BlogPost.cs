namespace BlogCommunityAPI_Assignment2.Repository.Entities
{
    public class BlogPost
    {
        public int PostID { get; set; }
        public int UserID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }  // Hämtas automatiskt
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
