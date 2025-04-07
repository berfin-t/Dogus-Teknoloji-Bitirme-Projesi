using BlogApp.Dtos.CommentDtos;

namespace BlogApp.Data.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task<CommentDto> CreateCommentAsync(CommentDto commentDto);
        IQueryable<CommentDto> Comments { get; }
        Task EditPost(CommentDto commentDto);
        Task DeleteCommentAsync(int commentId);
    }
}
