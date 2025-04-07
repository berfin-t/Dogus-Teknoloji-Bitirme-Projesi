using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlogApp.Dtos.CommentDtos;
using BlogApp.Entities;
using BlogApp.Models.BlogAppDbContext;
using BlogApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Repositories.Implementations.EfCore
{
    public class EfCoreCommentRepository:ICommentRepository
    {
        private readonly Context _context;
        private readonly IMapper _mapper;

        public EfCoreCommentRepository(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Create
        public async Task<CommentDto> CreateCommentAsync(CommentDto commentDto)
        {
            var entity = _mapper.Map<Comment>(commentDto);

            await _context.Comments.AddAsync(entity);
            await _context.SaveChangesAsync();

            commentDto.Id = entity.Id;
            return commentDto;
        }
        #endregion

        #region Read        
        public IQueryable<CommentDto> Comments => _context.Comments.AsNoTracking().ProjectTo<CommentDto>(_mapper.ConfigurationProvider); 
        #endregion

        #region Update
        public async Task EditPost(CommentDto commentDto)
        {
            var entity = await _context.Comments.FirstOrDefaultAsync(i => i.Id == commentDto.Id);

            if (entity == null) return;
            _mapper.Map(commentDto, entity);

            await _context.SaveChangesAsync();
        }
        #endregion

        #region Delete
        public async Task DeleteCommentAsync(int commentId)
        {
            var entity = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

            if (entity == null)
            {
                throw new InvalidOperationException($"{commentId} ID'li yorum bulunamadı.");
            }

            entity.IsDeleted = true;

            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
