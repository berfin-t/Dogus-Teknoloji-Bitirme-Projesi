using BlogApp.Dtos.CommentDtos;

namespace BlogApp.Data.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task<CommentCreateDto> CreateCommentAsync(CommentCreateDto commentDto);
        IQueryable<CommentDto> Comments { get; }
        Task EditPost(CommentDto commentDto);
        Task DeleteCommentAsync(int commentId);
    }
}
