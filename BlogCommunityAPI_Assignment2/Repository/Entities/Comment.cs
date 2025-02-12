namespace BlogCommunityAPI_Assignment2.Repository.Entities
{
    public class Comment
    {
        public int CommentID { get; set; }  // Genereras automatiskt i databasen
        public int PostID { get; set; }
        public int UserID { get; set; }  // Hämtas från JWT-token, ej från request
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
