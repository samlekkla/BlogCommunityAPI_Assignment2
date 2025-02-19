using BlogCommunityAPI_Assignment2.DTO;
using BlogCommunityAPI_Assignment2.Repository.Entities;
using BlogCommunityAPI_Assignment2.Repository.Interfaces;
using BlogCommunityAPI_Assignment2.Repository.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogCommunityAPI_Assignment2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ICommentRepository _commentRepository;

        public BlogPostController(IBlogPostRepository blogPostRepository, ICommentRepository commentRepository)
        {
            _blogPostRepository = blogPostRepository;
            _commentRepository = commentRepository;
        }

        [HttpGet("all")]
        public IActionResult GetAllBlogPosts()
        {
            var posts = _blogPostRepository.GetAllBlogPosts();
            return Ok(posts);
        }

        [HttpGet("categories")]
        public IActionResult GetAllCategories()
        {
            var categories = _blogPostRepository.GetAllCategories();
            return Ok(categories);
        }

        [Authorize]
        [HttpPost("CreateBlogPost")]
        public IActionResult CreateBlogPost([FromBody] CreateBlogPostDTO postDTO)
        {
            if (postDTO == null || string.IsNullOrWhiteSpace(postDTO.Title) || string.IsNullOrWhiteSpace(postDTO.Content))
            {
                return BadRequest("Invalid blog post data.");
            }

            //  Hämta UserID från den inloggade användaren
            var userIdClaim = User.FindFirst("UserID");
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found.");
            }

            int userId = int.Parse(userIdClaim.Value);

            //  Skapa och skicka DTO med UserID
            var newPost = new BlogPostDTO
            {
                Title = postDTO.Title,
                Content = postDTO.Content,
                CategoryID = postDTO.CategoryID,
                UserID = userId
            };

            //  Skicka till repository
            int postId = _blogPostRepository.CreateBlogPost(newPost);

            return Ok(new { Message = "Blog post created successfully.", PostID = postId });
        }

        [Authorize]
        [HttpPut("update/{postId}")]
        public IActionResult UpdateBlogPost(int postId, [FromBody] UpdateBlogPostDTO postDTO)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value); // Hämtar inloggad användares ID

            bool updated = _blogPostRepository.UpdateBlogPost(postId, postDTO, userId);

            if (updated)
            {
                return Ok(new { message = "Blogginlägget har uppdaterats." });
            }
            else
            {
                return Forbid(); // Om användaren inte är ägare av inlägget
            }
        }

        [Authorize]
        [HttpDelete("{postId}")]
        public IActionResult DeleteBlogPost(int postId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value); // Hämtar inloggad användares ID

            bool deleted = _blogPostRepository.DeleteBlogPost(postId, userId);

            if (!deleted)
                return NotFound(new { message = "Inlägget finns inte eller så har du inte behörighet att ta bort det." });

            return Ok(new { message = "Inlägget har raderats." });
        }



        [HttpGet("search/title")]
        public IActionResult SearchByTitle([FromQuery] string title)
        {
            var results = _blogPostRepository.SearchPostsByTitle(title);

            if (!results.Any())
            {
                return NotFound("Inga inlägg hittades.");
            }

            return Ok(results);
        }

        [HttpGet("search/category")]
        public IActionResult SearchByCategory([FromQuery] int categoryId)
        {
            var results = _blogPostRepository.SearchPostsByCategory(categoryId);

            if (!results.Any())
            {
                return NotFound("Inga inlägg hittades i denna kategori.");
            }

            return Ok(results);
        }



    }
}