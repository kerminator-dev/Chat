using Chat.API.DbContexts;
using Chat.API.Entities;
using Chat.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chat.API.Services.Implementation
{
    public class DatabaseUserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DatabaseUserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Создать пользователя
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Созданный пользователь</returns>
        public async Task<User> Create(User user)
        {
            user.RegisteredDateTime = DateTime.UtcNow;

            await _dbContext.Users.AddAsync(user);

            await _dbContext.SaveChangesAsync();

            return user;
        }

        /// <summary>
        /// Получить пользователя по Id
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Пользователь User</returns>
        public async Task<User?> Get(int userId)
        {
            return await _dbContext
                    .Users
                    .FindAsync(userId);
        }

        /// <summary>
        /// Получить пользователя по никнейму username
        /// </summary>
        /// <param name="username">Никнейм пользователя</param>
        /// <returns>Пользователь User</returns>
        public async Task<User?> Get(string username)
        {
            return await _dbContext
                    .Users
                    .FirstOrDefaultAsync(u => u.Username == username);
        }

        /// <summary>
        /// Получить список пользователь по коллекции из Id
        /// </summary>
        /// <param name="userIds">Коллекция идентификаторов пользователей</param>
        /// <returns>Коллекция найденных пользователей</returns>
        public async Task<ICollection<User>> Get(ICollection<int> userIds)
        {
            return await _dbContext.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
        }

        /// <summary>
        /// Найти пользователей по никнейму username
        /// </summary>
        /// <param name="username">Никнейм (чувствительный к регистру)</param>
        /// <returns>Список найденных пользователей</returns>
        public async Task<ICollection<User>> Search(string username, int count = 10)
        {
            return await _dbContext
                        .Users
                        .Where(u => u.Username.Contains(username))
                        .Take(count)
                        .ToListAsync();
        }

        /// <summary>
        /// Обновить данные о пользователе
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Пользователь User</returns>
        public async Task<User> Update(User user)
        {
            var result = _dbContext.Update(user).Entity;

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
