using AutoMapper;
using BlogApp.Dtos.CategoryDtos;
using BlogApp.Dtos.CommentDtos;
using BlogApp.Dtos.PostDtos;
using BlogApp.Dtos.UserDtos;
using BlogApp.Entities;

namespace BlogApp.Mapper
{
    public class BlogAppAutoMapperProfile:Profile
    {
        public BlogAppAutoMapperProfile()
        {
            CreateMap<Post, PostDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
