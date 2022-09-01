using Chat.API.DbContexts;
using Chat.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.API.Services.UserRepositories
{
    public class DatabaseUserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DatabaseUserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> Create(User user)
        {
            user.RegisteredDateTime = DateTime.UtcNow;

            await _dbContext.Users.AddAsync(user);

            await _dbContext.SaveChangesAsync();

            return user; 
        }

        public async Task<User> Get(int userId)
        {
            return await _dbContext
                    .Users
                    .FindAsync(userId);
        }

        public async Task<User> Get(string username)
        {
            return await _dbContext
                    .Users
                    .FirstOrDefaultAsync(u => u.Username == username);
        }

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
    }
}
