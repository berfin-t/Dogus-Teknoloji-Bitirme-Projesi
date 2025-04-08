namespace BlogApp.Dtos.UserDtos
{
    public class UserCreateDto
    {
        public string UserName { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; } = string.Empty;
        public string? UserProfile { get; set; }
    }
}
