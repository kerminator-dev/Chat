using Chat.API.DbContexts;
using Chat.API.Entities;

namespace Chat.API.Services.Interfaces
{
    public interface IConnectionRepository
    {
        Task<HubConnection> Get(string connectionId);

        Task Add(HubConnection connection);

        Task<ICollection<HubConnection>> Get(int userId);

        Task SetConnectionStatus(HubConnection hubConnection, bool isActiveStatus);
    }
}
