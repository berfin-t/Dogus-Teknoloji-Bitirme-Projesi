using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlogApp.Data.BlogAppDbContext;
using BlogApp.Data.Repositories.Interfaces;
using BlogApp.Dtos.UserDtos;
using BlogApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Repositories.Implementations.EfCore
{
    public class EfCoreUserRepository : IUserRepository
    {
        private readonly Context _context;
        private readonly IMapper _mapper;

        public EfCoreUserRepository(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Create
        public async Task<UserCreateDto> CreateUserAsync(UserCreateDto userDto)
        {
            var entity = _mapper.Map<User>(userDto);
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
            //userDto.Id = entity.Id; 

            return userDto;
        }
        #endregion

        #region Read        
        public IQueryable<UserDto> Users => _context.Users.AsNoTracking().ProjectTo<UserDto>(_mapper.ConfigurationProvider);
        #endregion

        #region Update
        public async Task EditUser(UserDto userDto)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(i => i.Id == userDto.Id);

            if (entity == null) return;
            _mapper.Map(userDto, entity);

            await _context.SaveChangesAsync();
        }
        #endregion

        #region Delete
        public async Task DeleteUserAsync(int userId)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(c => c.Id == userId);

            if (entity == null)
            {
                throw new InvalidOperationException($"{userId} ID'li user bulunamadı.");
            }

            entity.IsDeleted = true;

            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
