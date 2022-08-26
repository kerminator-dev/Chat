using System.ComponentModel.DataAnnotations;

namespace Chat.API.Models.Requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "User id is required!")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [MinLength(6, ErrorMessage = "Min password length is 6 characters!")]
        [MaxLength(20, ErrorMessage = "Max password length is 20 characters!")]
        public string Password { get; set; }
    }
}
