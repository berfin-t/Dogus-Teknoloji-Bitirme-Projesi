using BlogApp.Dtos.UserDtos;

namespace BlogApp.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDto> CreateUserAsync(UserDto userDto);
        IQueryable<UserDto> Users { get; }
        Task EditUser(UserDto userDto);
        Task DeleteUserAsync(int userId);
    }
}
