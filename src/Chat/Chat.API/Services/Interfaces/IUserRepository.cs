using Chat.API.Entities;

namespace Chat.API.Services.Interfaces
{
    public interface IUserRepository
    {
        Task<User> Get(int userId);

        Task<User> Get(string username);

        Task<ICollection<User>> Get(ICollection<int> userIds);

        Task<User> Create(User user);

        Task<User> Update(User user);

        Task<ICollection<User>> Search(string username, int count = 8);
    }
}
