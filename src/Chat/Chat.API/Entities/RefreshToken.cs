namespace Chat.API.Entities
{
    /// <summary>
    /// Refresh-токен
    /// </summary>
    public class RefreshToken
    {
        /// <summary>
        /// Id refresh-токена
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Refresh-токен
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Id пользователя-владельца refresh-токена
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Дата и время истекания refresh-токена
        /// </summary>
        public DateTime ExpirationDateTime { get; set; }
    }
}
