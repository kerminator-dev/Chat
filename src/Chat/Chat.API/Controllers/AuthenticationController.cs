using Chat.API.DTOs.Requests;
using Chat.API.DTOs.Responses;
using Chat.API.Entities;
using Chat.API.Exceptions;
using Chat.API.Services.Interfaces;
using Chat.API.Services.Providers;
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

        /// <summary>
        /// Зарегистрироваться
        /// </summary>
        /// <param name="registerRequest">Запрос на регистрацию</param>
        /// <returns></returns>
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            // Поиск пользователя с указанным username
            User existingUserByUsername = await _authenticationProvider.GetUser(registerRequest.Username.ToLower());
            if (existingUserByUsername != null)
            {
                // Если пользователь с таким username уже существует
                return BadRequestWithErrorOf<string>("Username already exists.");
            }

            try
            {
                // Регистрация пользователя
                await _authenticationProvider.RegisterUser(registerRequest);
            }
            catch (NotFoundException ex)
            {
                return NotFoundWithErrorOf<string>(ex.Message);
            }
            catch (ProcessingException ex)
            {
                return ConflictWithErrorOf<string>(ex.Message);
            }

            return Ok();
        }


        /// <summary>
        /// Выполнить вход в аккаунт
        /// </summary>
        /// <param name="loginRequest">Запрос на вход</param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            // Поиск пользователя по username
            User user = await _authenticationProvider.GetUser(loginRequest.Username.ToLower());
            if (user == null)
            {
                // Если такого пользователя не существует
                return UnauthorizedWithErrorOf<string>("Wrong username or password");
            }

            // Проверка пароля
            bool isCorrectPassword = _passwordHasher.VerifyPassword(loginRequest.Password, user.PasswordHash);
            if (!isCorrectPassword)
            {
                // Если пароль неверный
                return UnauthorizedWithErrorOf<string>("Wrong username or password");
            }

            try
            { 
                // Выполнение аутентификации
                AuthenticatedUserResponseDTO response = await _authenticationProvider.AuthenticateUser(user);

                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFoundWithErrorOf<string>(ex.Message);
            }
            catch (ProcessingException ex)
            {
                return ConflictWithErrorOf<string>(ex.Message);
            }
        }

        /// <summary>
        /// Поменять токен
        /// </summary>
        /// <param name="refreshTokenRequest">Запрос на смену токена</param>
        /// <returns></returns>
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO refreshTokenRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            // Проверка refresh-токена
            bool isValidRefreshToken = _authenticationProvider.ValidateRefreshToken(refreshTokenRequest.RefreshToken);
            if (!isValidRefreshToken)
            {
                // Если токен неверный
                return BadRequestWithErrorOf<string>("Invalid refresh token");
            }

            // Поиск токена в БД
            RefreshToken token = await _refreshTokenRepository.GetByToken(refreshTokenRequest.RefreshToken);
            if (token == null)
            {
                // Если токен не найден
                return BadRequestWithErrorOf<string>("Invalid refresh token");
            }

            // Удаление старого токена из БД
            await _refreshTokenRepository.Delete(token.Id);

            // Поиск пользователя
            User user = await _authenticationProvider.GetUser(token.UserId);  
            if (user == null)
            {
                // Если пользователь не найден
                return NotFoundWithErrorOf<string>("User not found");
            }

            try
            { 
                // Выполнение аутентификации/генерации токенов
                AuthenticatedUserResponseDTO response = await _authenticationProvider.AuthenticateUser(user);

                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFoundWithErrorOf<string>(ex.Message);
            }
            catch (ProcessingException ex)
            {
                return ConflictWithErrorOf<string>(ex.Message);
            }
        }
    }
}
