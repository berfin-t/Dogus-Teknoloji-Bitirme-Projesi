using BlogApp.Dtos.CategoryDtos;
using BlogApp.Dtos.CommentDtos;
using BlogApp.Dtos.UserDtos;

namespace BlogApp.Dtos.PostDtos
{
    public class PostDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Image { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; } = false;

        public int UserId { get; set; }
        public UserDto UserDto { get; set; } = null!;
        public int CategoryId { get; set; }
        public CategoryDto CategoryDto { get; set; } = null!;
        public List<CommentDto> CommentDtos { get; set; } = new List<CommentDto>();
    }
}
