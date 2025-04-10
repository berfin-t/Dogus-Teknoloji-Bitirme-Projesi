using AutoMapper.QueryableExtensions;
using AutoMapper;
using BlogApp.Dtos.CategoryDtos;
using BlogApp.Entities;
using Microsoft.EntityFrameworkCore;
using BlogApp.Data.BlogAppDbContext;
using BlogApp.Data.Repositories.Interfaces;

namespace BlogApp.Data.Repositories.Implementations.EfCore
{
    public class EfCoreCategoryRepository : ICategoryRepository
    {
        private readonly Context _context;
        private readonly IMapper _mapper;

        public EfCoreCategoryRepository(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Create
        public async Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto)
        {
            var entity = _mapper.Map<Category>(categoryDto);

            await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();

            categoryDto.Id = entity.Id;
            return categoryDto;
        }
        #endregion

        #region Read        
        public IQueryable<CategoryDto> Categories => _context.Categories.AsNoTracking().ProjectTo<CategoryDto>(_mapper.ConfigurationProvider);
        #endregion
        
    }
}
