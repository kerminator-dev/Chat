using Chat.API.Entities;

namespace Chat.API.Services.UserRepositories
{
    public interface IUserRepository
    {
        Task<User> GetByUserId(int userId);

        Task<User> Create(User user);
    }
}
