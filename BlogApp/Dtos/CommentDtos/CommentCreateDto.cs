using BlogApp.Dtos.PostDtos;
using BlogApp.Dtos.UserDtos;

namespace BlogApp.Dtos.CommentDtos
{
    public class CommentCreateDto
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; } = false;

        public int PostId { get; set; }
        public PostDto PostDto { get; set; } = null!;

        public int UserId { get; set; }
        public UserDto UserDto { get; set; } = null!;
    }
}
