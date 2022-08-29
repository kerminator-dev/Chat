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
            return await _dbContext.Users.FindAsync(userId);
        }

        public async Task<User> Get(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
