namespace BlogCommunityAPI_Assignment2.DTO
{
    public class CommentRequestDTO
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string CommentText { get; set; }
    }
}
