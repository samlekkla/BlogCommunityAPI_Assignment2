using BlogCommunityAPI_Assignment2.DTO;
using BlogCommunityAPI_Assignment2.Repository.Entities;

namespace BlogCommunityAPI_Assignment2.Repository.Interfaces
{
    public interface ICommentRepository
    {
        void AddComment(int postId, int userId, string commentText);
        List<Comment> GetCommentsByPost(int postId);
        int GetPostOwnerId(int postId);  // Hämta ägarens UserID för ett inlägg

    }
}
