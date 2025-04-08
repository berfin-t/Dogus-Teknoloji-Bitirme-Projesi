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
            CreateMap<Post, PostDto>()
            .ForMember(dest => dest.CategoryDto, opt => opt.MapFrom(src => src.Category))
            .ForMember(dest => dest.CommentDtos, opt => opt.MapFrom(src => src.Comments));

            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.UserDto, opt => opt.MapFrom(src => src.User))
                .ReverseMap();

            CreateMap<Category, CategoryDto>();

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserCreateDto>().ReverseMap();
        }
    }
}
