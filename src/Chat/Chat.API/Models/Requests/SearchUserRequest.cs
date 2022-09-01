using System.ComponentModel.DataAnnotations;

namespace Chat.API.Models.Requests
{
    public class SearchUserRequest
    {
        [Required(ErrorMessage = "Username is required!")]
        [MinLength(2, ErrorMessage = "Min username length is 2 characters!")]
        [MaxLength(20, ErrorMessage = "Max username length is 20 characters!")]
        public string Username { get; set; }
    }
}
