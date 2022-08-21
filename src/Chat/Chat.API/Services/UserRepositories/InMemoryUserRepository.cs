using Chat.API.Models;

namespace Chat.API.Services.UserRepositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = new List<User>();

        public Task<User> Create(User user)
        {
            user.Id = Guid.NewGuid();

            _users.Add(user);

            return Task.FromResult(user);
        }

        public Task<User> GetByUsername(string username)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.Username == username));
        }
    }
}
