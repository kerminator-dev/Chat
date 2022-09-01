using Chat.API.Entities;

namespace Chat.API.Services.UserRepositories
{
    public interface IUserRepository
    {
        Task<User> Get(int userId);
        
        Task<User> Get(string username);

        Task<ICollection<User>> Get(ICollection<int> userIds);

        Task<User> Create(User user);

        Task<ICollection<User>> Search(string username, int count = 8);
    }
}
