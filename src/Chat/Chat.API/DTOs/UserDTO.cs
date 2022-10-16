namespace Chat.API.Models.Responses
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class UserDTO
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Никнейм
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }
    }
}
