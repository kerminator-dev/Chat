using Chat.API.Models;
using Chat.API.Models.Requests;
using Chat.API.Models.Responses;
using Chat.API.Services.PasswordHashers;
using Chat.API.Services.UserRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public AuthenticationController(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));

                return BadRequest(new ErrorResponce(errorMessages));
            }

            User existingUserByUsername = await _userRepository.GetByUsername(registerRequest.Username);
            if (existingUserByUsername != null)
            {
                return Conflict(new ErrorResponce("Username already exists."));
            }

            string passwordHash = _passwordHasher.HashPassword(registerRequest.Password);
            User registrationUser = new User()
            {
                Username = registerRequest.Username,
                PasswordHash = passwordHash
            };

            await _userRepository.Create(registrationUser);

            return Ok();
        }
    }
}
