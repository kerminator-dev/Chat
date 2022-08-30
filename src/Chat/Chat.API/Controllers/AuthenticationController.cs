using Chat.API.Entities;
using Chat.API.Exceptions;
using Chat.API.Models.Requests;
using Chat.API.Models.Responses;
using Chat.API.Services.PasswordHashers;
using Chat.API.Services.Providers;
using Chat.API.Services.RefreshTokenRepositories;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controllers.ControllerBase
    {
        private readonly AuthenticationProvider _authenticationProvider;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthenticationController(IRefreshTokenRepository refreshTokenRepository, AuthenticationProvider authenticator, IPasswordHasher passwordHasher)
        {
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

            registerRequest.Username = registerRequest.Username.ToLower();
            User existingUserByUsername = await _authenticationProvider.GetUser(registerRequest.Username);
            if (existingUserByUsername != null)
            {
                return Conflict(new ErrorResponse("Username already exists."));
            }

            try
            {
                await _authenticationProvider.RegisterUser(registerRequest);
            }
            catch (ProcessingException ex)
            {
                return Conflict(new ErrorResponse(ex.Message));
            }

            return Ok();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            loginRequest.Username = loginRequest.Username.ToLower();
            User user = await _authenticationProvider.GetUser(loginRequest.Username);
            if (user == null)
            {
                return Unauthorized();
            }

            bool isCorrectPassword = _passwordHasher.VerifyPassword(loginRequest.Password, user.PasswordHash);
            if (!isCorrectPassword)
            {
                return Unauthorized();
            }

            try
            { 
                AuthenticatedUserResponse response = await _authenticationProvider.AuthenticateUser(user);

                return Ok(response);
            }
            catch (ProcessingException ex)
            {
                return Conflict(new ErrorResponse(ex.Message));
            }
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            bool isValidRefreshToken = _authenticationProvider.ValidateRefreshToken(refreshTokenRequest.RefreshToken);
            if (!isValidRefreshToken)
            {
                return BadRequest(new ErrorResponse("Invalid refresh token"));
            }

            RefreshToken token = await _refreshTokenRepository.GetByToken(refreshTokenRequest.RefreshToken);
            if (token == null)
            {
                return BadRequest(new ErrorResponse("Invalid refresh token"));
            }

            await _refreshTokenRepository.Delete(token.Id);

            User user = await _authenticationProvider.GetUser(token.UserId);  
            if (user == null)
            {
                return NotFound(new ErrorResponse("User not found"));
            }

            try
            { 
                AuthenticatedUserResponse response = await _authenticationProvider.AuthenticateUser(user);

                return Ok(response);
            }
            catch (ProcessingException ex)
            {
                return Conflict(new ErrorResponse(ex.Message));
            }
        }


    }
}
