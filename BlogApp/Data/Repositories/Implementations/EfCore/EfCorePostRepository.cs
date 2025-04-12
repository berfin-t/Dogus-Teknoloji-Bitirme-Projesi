using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlogApp.Data.BlogAppDbContext;
using BlogApp.Data.Repositories.Interfaces;
using BlogApp.Dtos.PostDtos;
using BlogApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Repositories.Implementations.EfCore
{
    public class EfCorePostRepository : IPostRepository
    {
        private readonly Context _context;
        private readonly IMapper _mapper;

        public EfCorePostRepository(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Create
        public async Task<PostDto> CreatePostAsync(PostDto postDto)
        {
            var entity = _mapper.Map<Post>(postDto);

            await _context.Posts.AddAsync(entity);
            await _context.SaveChangesAsync();

            postDto.Id = entity.Id;
            return postDto;
        }
        #endregion        

        #region Read        
        public IQueryable<PostDto> Posts
        {
            get
            {                
                return _context.Posts
                    .AsNoTracking()
                    .Where(p => !p.IsDeleted)
                    //.Include(p => p.Category)
                    .Include(p => p.User)
                    .ThenInclude(p => p.Comments)                   
                    .ProjectTo<PostDto>(_mapper.ConfigurationProvider);
            }
        }
        #endregion

        #region Update
        public async Task EditPost(PostDto postDto)
        {
            var entity = await _context.Posts.FirstOrDefaultAsync(i => i.Id == postDto.Id);

            if(entity != null)
            {
                entity.Title = postDto.Title;
                entity.Content = postDto.Content;
                entity.IsActive = postDto.IsActive;
                entity.CategoryId = postDto.CategoryId;
                if (!string.IsNullOrEmpty(postDto.Image))
                {
                    entity.Image = postDto.Image;
                }
            }   
                       
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Delete
        public async Task DeletePostAsync(int postId)
        {
            var entity = await _context.Posts.FirstOrDefaultAsync(c => c.Id == postId);

            if (entity == null)
            {
                throw new InvalidOperationException($"{postId} ID'li post bulunamadı.");
            }

            entity.IsDeleted = true;

            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
