using Chat.API.Entities;
using Chat.API.Services.ConnectionRepositories;
using Chat.API.Services.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Chat.API.Hubs
{
    /// <summary>
    /// Хаб, расчитанный на рассылку сообщений пользователям
    /// </summary>
    public class ChatHub : Hub
    {
        private readonly AuthenticationProvider _authenticationProvider;
        private readonly IConnectionRepository _connectionRepository;

        public ChatHub(AuthenticationProvider authenticationProvider, IConnectionRepository connectionRepository)
        {
            _authenticationProvider = authenticationProvider;
            _connectionRepository = connectionRepository;
        }

        /// <summary>
        /// При подключении пользователя к хабу
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public override async Task OnConnectedAsync()
        {
            // Получение пользователя из Context'а
            User user = await _authenticationProvider.GetHttpContextUser(Context.User);
            if (user == null)
                return;

            // Подгрузка подключений пользователя
            await _connectionRepository.LoadConnections(user);

            // Получение User-Agent'а пользователя
            string userAgent = Context.GetHttpContext()?
                                .Request
                                .Headers["User-Agent"]
                                .FirstOrDefault(string.Empty) ?? "unknown user-agent";

            // Иницилизация модели подключения
            var connection = new HubConnection()
            {
                Id = Context.ConnectionId,
                UserId = user.Id,
                UserAgent = userAgent ?? string.Empty,
                Connected = true // Пометка как активное подключение
            };

            // Добавление текущего подключения в базу данных
            await _connectionRepository.Add(user, connection);

            await base.OnConnectedAsync();
        }

        /// <summary>
        /// При отключении от хаба
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        [Authorize]
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // Получение пользователя из Context'а
            User user = await _authenticationProvider.GetHttpContextUser(Context.User);
            if (user == null)
                return;

            // Получение текущего подключения из БД
            var connection = await _connectionRepository.Get(Context.ConnectionId);
            if (connection == null)
                return;

            // Пометка подключения как неактивного
            await _connectionRepository.SetConnectionStatus(connection, isActiveStatus: false);

            await base.OnDisconnectedAsync(exception);
        }

    }
}
