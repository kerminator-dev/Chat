using Chat.API.Models;
using Chat.API.Models.Requests;
using Chat.API.Models.Responses;
using Chat.API.Services.Authenticators;
using Chat.API.Services.PasswordHashers;
using Chat.API.Services.RefreshTokenRepositories;
using Chat.API.Services.TokenValidators;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly AuthenticationProvider _authenticationProvider;
        private readonly RefreshTokenValidator _refreshTokenValidator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthenticationController(RefreshTokenValidator refreshTokenValidator, IRefreshTokenRepository refreshTokenRepository, AuthenticationProvider authenticator, IPasswordHasher passwordHasher)
        {
            _refreshTokenValidator = refreshTokenValidator;
            _refreshTokenRepository = refreshTokenRepository;
            _authenticationProvider = authenticator;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            User existingUserByUsername = await _authenticationProvider.GetUser(registerRequest.UserId);
            if (existingUserByUsername != null)
            {
                return Conflict(new ErrorResponse("Username already exists."));
            }

            await _authenticationProvider.RegisterUser(registerRequest);

            return Ok();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            User user = await _authenticationProvider.GetUser(loginRequest.UserId);
            if (user == null)
            {
                return Unauthorized();
            }

            bool isCorrectPassword = _passwordHasher.VerifyPassword(loginRequest.Password, user.PasswordHash);
            if (!isCorrectPassword)
            {
                return Unauthorized();
            }

            AuthenticatedUserResponce response = await _authenticationProvider.AuthenticateUser(user);

            return Ok(response);
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            bool isValidRefreshToken = _refreshTokenValidator.Validate(refreshTokenRequest.RefreshToken);
            if (!isValidRefreshToken)
            {
                return BadRequest(new ErrorResponse("Invalid refresh token"));
            }

            RefreshToken tonen = await _refreshTokenRepository.GetByToken(refreshTokenRequest.RefreshToken);
            if (tonen == null)
            {
                return NotFound(new ErrorResponse("Invalid refresh token"));
            }

            await _refreshTokenRepository.Delete(tonen.Id);

            User user = await _authenticationProvider.GetUser(tonen.UserId);  
            if (user == null)
            {
                return NotFound(new ErrorResponse("User not found"));
            }

            AuthenticatedUserResponce response = await _authenticationProvider.AuthenticateUser(user);

            return Ok(response);
        }

        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));

            return BadRequest(new ErrorResponse(errorMessages));
        }
    }
}
