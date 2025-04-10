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
            .ForMember(dest => dest.CommentDtos, opt => opt.MapFrom(src => src.Comments))
            .ForMember(dest => dest.UserDto, opt => opt.MapFrom(src => src.User));

            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.UserDto, opt => opt.MapFrom(src => src.User))
                .ReverseMap();
            CreateMap<CommentDto, CommentCreateDto>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ReverseMap();          

            CreateMap<CommentCreateDto, Comment>().ReverseMap();    

            CreateMap<Category, CategoryDto>();

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.PostDtos, opt => opt.MapFrom(src => src.Posts)); ;
            CreateMap<User, UserCreateDto>().ReverseMap();          

           
        }
    }
}
