namespace Chat.API.Entities
{
    public class Connection
    {
        public string ConnectionId { get; set; }
        public int UserId { get; set; }
        public string UserAgent { get; set; }
        public bool Connected { get; set; }
    }
}
