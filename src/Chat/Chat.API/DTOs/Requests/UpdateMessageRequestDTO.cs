using System.ComponentModel.DataAnnotations;

namespace Chat.API.DTOs.Requests
{
    public class UpdateMessageRequestDTO
    {
        [Required(ErrorMessage = "Dialogue is required!")]
        public int DialogueId { get; set; }

        [Required(ErrorMessage = "Message is required!")]
        public int MessageId { get; set; }

        [Required(ErrorMessage = "Message content is required!")]
        [MinLength(1)]
        [MaxLength(5000)]
        public string Content { get; set; }
    }
}
