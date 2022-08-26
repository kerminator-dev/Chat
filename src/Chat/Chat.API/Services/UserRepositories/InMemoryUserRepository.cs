using Chat.API.Entities;

namespace Chat.API.Services.UserRepositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = new List<User>();

        public Task<User> Create(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (_users.Any(u => u.UserId == user.UserId))
                throw new ArithmeticException("User with this username already exist");

            user.RegisteredDateTime = DateTime.UtcNow;

            _users.Add(user);

            return Task.FromResult(user);
        }

        public Task<User> GetByUserId(int userId)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.UserId == userId));
        }
    }
}
