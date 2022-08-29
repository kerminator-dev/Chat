using Chat.API.DbContexts;
using Chat.API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace Chat.API.Hubs
{
    /// <summary>
    /// Хаб, расчитанный на рассылку сообщений пользователям
    /// </summary>
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _dbContext;

        public ChatHub(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
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

            string userAgent = Context.GetHttpContext()?
                                .Request
                                .Headers["User-Agent"]
                                .FirstOrDefault(string.Empty) ?? String.Empty;

            user.Connections.Add
            (
                new HubConnection
                {
                    Id = Context.ConnectionId,
                    UserId = user.Id,
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

            var connection = await _dbContext.Connections.FindAsync(Context.ConnectionId);

            if (connection == null)
                return;

            connection.Connected = false;

            _dbContext.SaveChanges();
        }

    }
}
