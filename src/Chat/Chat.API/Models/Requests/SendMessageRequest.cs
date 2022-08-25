namespace Chat.API.Models.Requests
{
    public class SendMessageRequest
    {
        public string ReceiverId { get; set; }
        public string Content { get; set; }
    }
}
