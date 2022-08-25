namespace Chat.API.Models
{
    public class Connection
    {
        public string ConnectionID { get; set; }
        public string UserId { get; set; }
        public string UserAgent { get; set; }
        public bool Connected { get; set; }
    }
}
