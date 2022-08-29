using Chat.API.DbContexts;
using Chat.API.Entities;

namespace Chat.API.Services.ConnectionRepositories
{
    public interface IConnectionRepository
    {


        Task<ICollection<HubConnection>> GetUserConnections(int userId);
    }
}
