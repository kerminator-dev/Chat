namespace Chat.API.Entities
{
    /// <summary>
    /// Подключение к SignalR-хабу
    /// </summary>
    public class HubConnection
    {
        /// <summary>
        /// Id подключения
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Id пользователя
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// UserAgent пользователя
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Статус подключения
        /// true - подключился к хабу
        /// false - отключился от хаба
        /// </summary>
        public bool Connected { get; set; }
    }
}
