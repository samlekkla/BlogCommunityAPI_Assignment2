namespace BlogCommunityAPI_Assignment2.DTO
{
    public class LoginUserDTO
    {
        public string Username { get; set; } // Username for login
        public string PasswordHash { get; set; } // Plain-text password for verification

    }
}
