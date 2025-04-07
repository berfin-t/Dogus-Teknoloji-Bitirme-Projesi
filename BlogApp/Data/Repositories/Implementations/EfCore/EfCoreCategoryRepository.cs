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
        //AsNoTracking(). ile salt okunur işlemler için izlemeyi devre dışı bırakma
        //Sonuçları doğrudan CategoryDtokullanarak yansıtmakProjectTo , Categorygereksiz veri yüklemeden varlıkları veritabanı düzeyindeki ilgili
        //DTO'lara verimli bir şekilde eşler.
        public IQueryable<CategoryDto> Categories => _context.Categories.AsNoTracking().ProjectTo<CategoryDto>(_mapper.ConfigurationProvider);
        #endregion

        #region Update
        public async Task EditPost(CategoryDto categoryDto)
        {
            var entity = await _context.Categories.FirstOrDefaultAsync(i => i.Id == categoryDto.Id);

            if (entity == null) return;
            _mapper.Map(categoryDto, entity);

            await _context.SaveChangesAsync();
        }
        #endregion

        #region Delete
        public async Task DeleteCategoryAsync(int categoryId)
        {
            var entity = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);

            if (entity == null)
            {
                throw new InvalidOperationException($"{categoryId} ID'li kategori bulunamadı.");
            }

            entity.IsDeleted = true;

            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
