namespace Chat.API.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Name { get; set; }
        public DateTime RegisteredDateTime { get; set; }

        /// <summary>
        /// Список SignalR-подключений к хабу
        /// </summary>
        public virtual ICollection<HubConnection> Connections { get; set; }

        /// <summary>
        /// Список бесед пользователя
        /// </summary>
        public virtual ICollection<Conversation> Conversations { get; set; }
    }
}
