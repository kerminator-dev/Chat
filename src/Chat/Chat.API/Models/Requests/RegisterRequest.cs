using System.ComponentModel.DataAnnotations;

namespace Chat.API.Models.Requests
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Username is required")]
        [MinLength(4, ErrorMessage = "Min password length is 4 characters!")]
        [MaxLength(10, ErrorMessage = "Max password length is 20 characters!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Min password length is 8 characters!")]
        [MaxLength(20, ErrorMessage = "Max password length is 20 characters!")]
        public string Password { get; set; }
    }
}
