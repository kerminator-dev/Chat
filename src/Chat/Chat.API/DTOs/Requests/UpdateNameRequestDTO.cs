using System.ComponentModel.DataAnnotations;

namespace Chat.API.DTOs.Requests
{
    public class UpdateNameRequestDTO
    {
        [Required(ErrorMessage = "Given name is required!")]
        [MinLength(2, ErrorMessage = "Min name length is 2 characters!")]
        [MaxLength(20, ErrorMessage = "Max name length is 20 characters!")]
        public string Name { get; set; }
    }
}
