namespace BlogCommunityAPI_Assignment2.DTO
{
    public class CommentDTO
    {
        public int CommentID { get; set; }
        public int PostID { get; set; }
        public int UserID { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
