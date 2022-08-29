using Chat.API.Entities;

namespace Chat.API.Models.Responses
{
    public class DialoguesResponse
    {
        public int UserId { get; set; }

        public ICollection<Dialogue> Dialogues { get; set; }

        public int Count => Dialogues.Count;
    }
}
