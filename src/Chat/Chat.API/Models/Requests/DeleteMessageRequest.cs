using System.ComponentModel.DataAnnotations;

namespace Chat.API.Models.Requests
{
    public class DeleteMessageRequest
    {
        [Required(ErrorMessage = "Dialogue is required!")]
        public int DialogueId { get; set; }

        [Required(ErrorMessage = "Message is required!")]
        public int MessageId { get; set; }
    }
}
