using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlogApp.Data.BlogAppDbContext;
using BlogApp.Data.Repositories.Interfaces;
using BlogApp.Dtos.CommentDtos;
using BlogApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Repositories.Implementations.EfCore
{
    public class EfCoreCommentRepository : ICommentRepository
    {
        private readonly Context _context;
        private readonly IMapper _mapper;

        public EfCoreCommentRepository(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Create
        public async Task<CommentCreateDto> CreateCommentAsync(CommentCreateDto commentDto)
        {
            var commentEntity = _mapper.Map<Comment>(commentDto);

            await _context.Comments.AddAsync(commentEntity);
            await _context.SaveChangesAsync();

            var createdCommentEntity = await _context.Comments
                .Include(c => c.User)  
                .Where(c => c.Id == commentEntity.Id)  
                .FirstOrDefaultAsync();

            var createdCommentDto = _mapper.Map<CommentCreateDto>(createdCommentEntity);

            return createdCommentDto;
        }
        #endregion

        #region Read        
        public IQueryable<CommentDto> Comments => _context.Comments.AsNoTracking().ProjectTo<CommentDto>(_mapper.ConfigurationProvider);
        #endregion

        #region Update
        public async Task EditCommentAsync(CommentDto commentDto)
        {
            var entity = await _context.Comments.FirstOrDefaultAsync(i => i.Id == commentDto.Id);

            if (entity == null) return;

            entity.Text = commentDto.Text;

            //_mapper.Map(commentDto, entity).Text;

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
