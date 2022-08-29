namespace Chat.API.Entities
{
    public class HubConnection
    {
        public string Id { get; set; }
        public int UserId { get; set; }
        public string UserAgent { get; set; }
        public bool Connected { get; set; }
    }
}
