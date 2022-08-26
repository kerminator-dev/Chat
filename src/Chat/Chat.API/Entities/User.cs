namespace Chat.API.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string PasswordHash { get; set; }
        public string GivenName { get; set; }
        public DateTime RegisteredDateTime { get; set; }

        /// <summary>
        /// Список SignalR-подключений к хабу
        /// </summary>
        public virtual ICollection<Connection> Connections { get; set; }

        /// <summary>
        /// Список диалогов пользователя
        /// </summary>
        public virtual ICollection<Conversation> Conversations { get; set; }
    }
}
