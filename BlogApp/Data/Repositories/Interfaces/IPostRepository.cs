﻿using BlogApp.Dtos.PostDtos;

namespace BlogApp.Data.Repositories.Interfaces
{
    public interface IPostRepository
    {
        Task<PostDto> CreatePostAsync(PostDto postDto);
        IQueryable<PostDto> Posts { get; }
        Task EditPost(PostDto postDto);
        Task DeletePostAsync(int postId);
    }
}
