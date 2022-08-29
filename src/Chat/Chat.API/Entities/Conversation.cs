namespace Chat.API.Entities
{
    public class Conversation
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }


        /// <summary>
        /// Список пользователей диалога
        /// </summary>
        public virtual ICollection<User> Members { get; set; }
    }
}
