using System.ComponentModel.DataAnnotations;

namespace Chat.API.Models.Requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Username is required")]
        [MinLength(6, ErrorMessage = "Min password length is 4 characters!")]
        [MaxLength(20, ErrorMessage = "Max password length is 20 characters!")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Min password length is 8 characters!")]
        [MaxLength(20, ErrorMessage = "Max password length is 20 characters!")]
        public string Password { get; set; }
    }
}
