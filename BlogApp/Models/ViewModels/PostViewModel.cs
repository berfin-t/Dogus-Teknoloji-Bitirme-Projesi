using BlogApp.Dtos.PostDtos;
using BlogApp.Entities;

namespace BlogApp.Models.ViewModels
{
    public class PostViewModel
    {
        public List<PostDto> Posts { get; set; } = new List<PostDto>();
        public int TotalPosts { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalPosts / (double)PageSize);

        public string? SelectedCategory { get; set; }
        public List<string> Categories { get; set; } = new();
    }
}
