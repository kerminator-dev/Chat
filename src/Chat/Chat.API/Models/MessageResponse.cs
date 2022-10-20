namespace Chat.API.Models
{
    public abstract class MessagesResponse<T>
    {
        protected IEnumerable<T> Messages { get; set; }

        protected MessagesResponse(T message) 
            : this(new List<T>() { message }) { }

        protected MessagesResponse(IEnumerable<T> messages)
        {
            Messages = messages;
        }
    }
}
