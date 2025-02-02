namespace BlogCommunityAPI_Assignment2.DTO
{
    public class CreateUserDTO
    {
        public string Username { get; set; } // Username for the new user
        public string PasswordHash { get; set; } // Plain-text password (to be hashed)
        public string Email { get; set; } // Email for the new user
    }
}
