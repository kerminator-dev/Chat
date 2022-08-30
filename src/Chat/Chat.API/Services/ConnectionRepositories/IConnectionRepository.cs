using Chat.API.DbContexts;
using Chat.API.Entities;

namespace Chat.API.Services.ConnectionRepositories
{
    public interface IConnectionRepository
    {

        Task<User> LoadConnections(User user);
        
        Task<HubConnection> Get(string connectionId);

        Task Add(User user, HubConnection connection);

        Task<ICollection<HubConnection>> GetUserConnections(int userId);

        Task SetConnectionStatus(HubConnection hubConnection, bool isActiveStatus);
    }
}
