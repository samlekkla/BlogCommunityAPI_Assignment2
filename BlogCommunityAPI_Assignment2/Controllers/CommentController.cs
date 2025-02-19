using BlogCommunityAPI_Assignment2.DTO;
using BlogCommunityAPI_Assignment2.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogCommunityAPI_Assignment2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;

        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpPost("add")]
        public IActionResult AddComment([FromBody] CommentDTO commentDTO)
        {
            if (commentDTO == null || string.IsNullOrWhiteSpace(commentDTO.CommentText))
            {
                return BadRequest("Invalid comment request.");
            }

            // Hämta inloggade användarens ID från token
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("UserID");
            if (currentUserIdClaim == null)
            {
                return Unauthorized("User not authenticated.");
            }

            int currentUserId = int.Parse(currentUserIdClaim.Value);

            // Hämta inläggets ägare (postOwnerId)
            var postOwnerId = _commentRepository.GetPostOwnerId(commentDTO.PostID);

            if (postOwnerId == -1)
            {
                return NotFound("Post not found.");
            }

            // Kontrollera att användaren inte kommenterar sitt eget inlägg
            if (currentUserId == postOwnerId)
            {
                return BadRequest("You cannot comment on your own post.");
            }

            // Lägg till kommentaren
            _commentRepository.AddComment(commentDTO.PostID, currentUserId, commentDTO.CommentText);

            return Ok("Comment added successfully.");
        }

        [HttpGet("post/{postId}")]
        public IActionResult GetCommentsByPost(int postId)
        {
            var comments = _commentRepository.GetCommentsByPost(postId);
            if (comments == null || comments.Count == 0)
            {
                return NotFound("No comments found for this post.");
            }
            return Ok(comments);
        }

        [HttpGet("debug-token")]
        public IActionResult DebugToken()
        {
            var claims = User.Claims.Select(c => $"{c.Type}: {c.Value}").ToList();
            return Ok(new { Message = "Token Received", Claims = claims });
        }
    }
}
