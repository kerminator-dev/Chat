using Chat.API.Entities;

namespace Chat.API.Services.UserRepositories
{
    public interface IUserRepository
    {
        Task<User> Get(int userId);
        
        Task<User> Get(string username);

        Task<User> Create(User user);
    }
}
