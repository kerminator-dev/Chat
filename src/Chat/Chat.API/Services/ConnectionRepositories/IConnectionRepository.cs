using Chat.API.DbContexts;
using Chat.API.Models;

namespace Chat.API.Services.ConnectionRepositories
{
    public interface IConnectionRepository
    {


        Task<ICollection<Connection>> GetUserConnections(string userId);
    }
}
