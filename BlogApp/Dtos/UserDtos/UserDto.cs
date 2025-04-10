using BlogApp.Dtos.CommentDtos;
using BlogApp.Dtos.PostDtos;
using BlogApp.Entities;

namespace BlogApp.Dtos.UserDtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? UserProfile { get; set; }
        public bool IsDeleted { get; set; } = false;

        public List<PostDto> PostDtos { get; set; } = new();
        public List<CommentDto> CommentDtos { get; set; } = new List<CommentDto>();
    }
}
