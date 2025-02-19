using BlogCommunityAPI_Assignment2.DTO;
using BlogCommunityAPI_Assignment2.Repository.Entities;

namespace BlogCommunityAPI_Assignment2.Repository.Interfaces
{
    public interface IBlogPostRepository
    {
        int CreateBlogPost(BlogPostDTO post);
        bool UpdateBlogPost(int postId, UpdateBlogPostDTO post, int userId);
        bool DeleteBlogPost(int postId, int userId);
        IEnumerable<BlogPostDTO> GetAllBlogPosts();
        IEnumerable<BlogPostDTO> GetBlogPostsByUser(int userId);
        IEnumerable<CategoryDTO> GetAllCategories(); // Ny metod
        BlogPostDTO GetBlogPostById(int postId);
        IEnumerable<BlogPostDTO> SearchPostsByTitle(string title);
        IEnumerable<BlogPostDTO> SearchPostsByCategory(int categoryId);


    }
}
