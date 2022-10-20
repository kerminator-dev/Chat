using System.ComponentModel.DataAnnotations;

namespace Chat.API.DTOs.Requests
{
    public class SendMessageRequestDTO
    {
        [Required(ErrorMessage ="Dialogue id is required!")]
        public int DialogueId { get; set; }

        [Required(ErrorMessage ="Message content is required!")]
        [MinLength(1)]
        [MaxLength(5000)]
        public string Content { get; set; }
    }
}
