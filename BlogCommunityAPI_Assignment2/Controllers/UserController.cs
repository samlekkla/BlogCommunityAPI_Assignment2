using AutoMapper;
using BlogCommunityAPI_Assignment2.DTO;
using BlogCommunityAPI_Assignment2.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogCommunityAPI_Assignment2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IBlogCommunity _dbContext;

        public UserController(IUserRepository userRepository, IMapper mapper, IBlogCommunity dbContext)
        {

            _userRepository = userRepository;
            _mapper = mapper;
            _dbContext = dbContext;

        }

        [AllowAnonymous]
        // Create a new user
        [HttpPost("register_New_User")]
        public IActionResult Register(CreateUserDTO createUserDTO)
        {
            var user = _mapper.Map<User>(createUserDTO);

            var userId = _userRepository.CreateUser(user.Username, user.PasswordHash, user.Email); // Get the correct UserId

            // Set the UserId directly in the mapped user object
            user.UserID = userId;
            var userDTO = _mapper.Map<UserDTO>(user); // Map after setting the ID

            return CreatedAtAction(nameof(GetUser), new { username = user.Username }, userDTO); // Use CreatedAtAction for 201
        }

        // Get a user by username
        [HttpGet("{username}/Get_a_user_by_username")]
        public IActionResult GetUser(string username)
        {
            // Retrieve the user from the repository
            var user = _userRepository.GetUserByUsername(username);

            if (user == null)
            {
                // Return 404 if the user is not found
                return NotFound("User not found.");
            }

            // Map the user to a DTO
            var userDTO = _mapper.Map<UserDTO>(user);
            return Ok(userDTO);
        }

        [HttpPut("{userId}/Update_User")]
        public IActionResult UpdateUser(int userId, [FromQuery] UpdateUserDTO userDTO)
        {
            if (userDTO == null)
            {
                return BadRequest("The UserID field is required.");
            }

            try
            {
                // Call the repository method to update the auction
                var rowsAffected = _userRepository.UpdateUser(userId, userDTO);

                if (rowsAffected == 0)
                {
                    return NotFound("No found for the given user ID.");
                }

                return Ok("User updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                // Return a Bad Request for business logic errors
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, new { Error = "An unexpected error occurred.", Details = ex.Message });
            }
        }


        [HttpDelete("{userId}/Delete_User")]
        public IActionResult DeleteUser(int userId)
        {
            var rowsAffected = _userRepository.DeleteUser(userId);

            if (rowsAffected == 0)
            {
                return NotFound("No found for the given user ID");
            }

            return Ok("User account deleted successfully.");
        }

    }
}
