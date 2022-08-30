using Chat.API.DbContexts;
using Chat.API.Entities;

namespace Chat.API.Services.ConnectionRepositories
{
    public interface IConnectionRepository
    {

        Task<User> LoadConnections(User user);
        Task<ICollection<HubConnection>> GetUserConnections(int userId);
    }
}
