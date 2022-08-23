using Chat.API.Models;

namespace Chat.API.Services.UserRepositories
{
    public interface IUserRepository
    {
        Task<User> GetByUserId(string userId);

        Task<User> Create(User user);
    }
}
