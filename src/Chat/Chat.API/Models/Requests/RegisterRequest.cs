using System.ComponentModel.DataAnnotations;

namespace Chat.API.Models.Requests
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Username is required!")]
        [MinLength(4, ErrorMessage = "Min username length is 4 characters!")]
        [MaxLength(10, ErrorMessage = "Max username length is 10 characters!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [MinLength(6, ErrorMessage = "Min password length is 6 characters!")]
        [MaxLength(20, ErrorMessage = "Max password length is 20 characters!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Given name is required!")]
        [MinLength(2, ErrorMessage = "Min name length is 2 characters!")]
        [MaxLength(20, ErrorMessage = "Max name length is 20 characters!")]
        public string Name { get; set; }
    }
}
