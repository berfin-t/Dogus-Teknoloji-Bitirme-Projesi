using BlogApp.Dtos.CategoryDtos;

namespace BlogApp.Data.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto);
        IQueryable<CategoryDto> Categories { get; }
    }
}
