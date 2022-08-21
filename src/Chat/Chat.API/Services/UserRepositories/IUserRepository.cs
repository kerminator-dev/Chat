using Chat.API.Models;

namespace Chat.API.Services.UserRepositories
{
    public interface IUserRepository
    {
        Task<User> GetByUsername(string username);

        Task<User> Create(User user);
    }
}
