namespace Chat.API.Models
{
    public class User
    {
        public string UserId { get; set; }
        public string PasswordHash { get; set; }
        public string GivenName { get; set; }
        public DateTime RegisteredDateTime { get; set; }

        public virtual ICollection<Connection> Connections { get; set; }
    }
}
