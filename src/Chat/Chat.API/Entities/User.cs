namespace Chat.API.Entities
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Никнейм пользователя
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Хэш пароля
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Дата и время регистрация пользователя
        /// </summary>
        public DateTime RegisteredDateTime { get; set; }

        /// <summary>
        /// Список SignalR-подключений к хабу
        /// </summary>
        public virtual ICollection<HubConnection> HubConnections { get; set; }
    }
}
