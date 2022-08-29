using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Chat.API.Models.Requests
{
    public class GetMessagesRequest
    {
        [Required(ErrorMessage = "Dialogue is required!")]
        public int DialogueId { get; set; }

        [Required]
        [Range(10, 50, ErrorMessage = "Count of messages should be from 10 to 50!")]
        public int Count { get; set; }

        [Required]
        [DefaultValue(0)]
        public int Offset { get; set; }
    }
}
