using System.Drawing;

namespace Chat.API.DTOs.Responses
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

        /// <summary>
        /// Цвет
        /// </summary>
        public int Color { get; set; }
    }
}
