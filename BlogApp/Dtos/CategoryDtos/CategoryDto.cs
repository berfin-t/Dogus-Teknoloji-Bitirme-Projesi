using BlogApp.Dtos.PostDtos;

namespace BlogApp.Dtos.CategoryDtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsDeleted { get; set; } = false;

        public List<PostDto> Posts { get; set; } = new List<PostDto>();
    }
}
