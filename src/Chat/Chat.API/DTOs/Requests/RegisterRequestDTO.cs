using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Chat.API.DTOs.Requests
{
    public class RegisterRequestDTO
    {
        [Required(ErrorMessage = "Username is required!")]
        [MinLength(3, ErrorMessage = "Min username length is 3 characters!")]
        [MaxLength(16, ErrorMessage = "Max username length is 16 characters!")]
        [RegularExpression(@"^[a-zA-Z0-9]{3,20}$")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [MinLength(8, ErrorMessage = "Min password length is 8 characters!")]
        [MaxLength(20, ErrorMessage = "Max password length is 20 characters!")]
        [RegularExpression(@"^[a-zA-Z0-9]{3,20}$", ErrorMessage = "Password may contains digits and letters. Length of password is 3-20 characters!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Given name is required!")]
        [MinLength(2, ErrorMessage = "Min name length is 2 characters!")]
        [MaxLength(20, ErrorMessage = "Max name length is 20 characters!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Color is required!")]
        public int Color { get; set; }
    }
}
