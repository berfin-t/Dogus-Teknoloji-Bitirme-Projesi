using BlogApp.Dtos.UserDtos;

namespace BlogApp.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<UserCreateDto> CreateUserAsync(UserCreateDto userDto);
        IQueryable<UserDto> Users { get; }
        Task EditUser(UserDto userDto);
        Task DeleteUserAsync(int userId);
        Task<UserDto> GetUserWithNameAsync(string userName);
    }
}
