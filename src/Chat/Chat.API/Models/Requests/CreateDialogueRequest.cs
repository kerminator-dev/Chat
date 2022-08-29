using System.ComponentModel.DataAnnotations;

namespace Chat.API.Models.Requests
{
    public class CreateDialogueRequest
    {
        [Required(ErrorMessage = "User is required!")]
        public int TargetUserId { get; set; }
    }
}
