using Chat.API.Entities;

namespace Chat.API.Models.Responses
{
    public class GetMessagesResponse
    {
        public ICollection<DialogueMessage> Messages { get; set; }

        public int Count => Messages.Count;
    }
}
