namespace TaskManagement.DTOs
{
    public class UserLoginDto
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

}
