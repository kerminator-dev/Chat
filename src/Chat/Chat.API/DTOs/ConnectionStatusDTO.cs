namespace Chat.API.DTOs
{
    /// <summary>
    /// Статус подключения пользователя (online/offline)
    /// </summary>
    public class ConnectionStatusDTO
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Статус подключения
        /// true - подключен
        /// false - не подключен
        /// </summary>
        public bool IsConnected { get; set; }
    }
}
