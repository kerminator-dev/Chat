using Chat.API.DbContexts;
using Chat.API.Models;
using Chat.API.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace Chat.API.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _dbContext;

        public ChatHub(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize]
        public async Task SendMessage(SendMessageRequest message)
        {
            
        }

        public override async Task OnConnectedAsync()
        {
            string userId = Context.User.FindFirstValue("id");

            if (userId == null)
                return;

            User user = await _dbContext.Users.FindAsync(userId);

            if (user == null)
                return;




            await _dbContext.Entry(user)
                .Collection(u => u.Connections)
                .LoadAsync();

            string userAgent = Context.GetHttpContext()
                                .Request
                                .Headers["User-Agent"]
                                .FirstOrDefault(string.Empty);

            user.Connections.Add
            (
                new Connection
                {
                    ConnectionID = Context.ConnectionId,
                    UserId = user.UserId,
                    UserAgent = userAgent ?? string.Empty,
                    Connected = true
                }
            );

            await _dbContext.SaveChangesAsync();

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string userId = Context.User.FindFirstValue("id");

            if (userId == null)
                return;

            User user = await _dbContext.Users.FindAsync(userId);

            if (user == null)
                return;

            var connection = _dbContext.Connections.Find(Context.ConnectionId);

            if (connection == null)
                return;

            _dbContext.Connections.Remove(connection);


            _dbContext.SaveChanges();
        }

    }
}
