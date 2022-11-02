using Chat.API.DTOs.Requests;
using Chat.API.Exceptions;
using Chat.API.Services.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly AuthenticationProvider _authenticationProvider;
        private readonly UserProvider _userProvider;

        public ProfileController(AuthenticationProvider authenticationProvider, UserProvider userProvider)
        {
            _authenticationProvider = authenticationProvider;
            _userProvider = userProvider;
        }

        /// <summary>
        /// Обновить пароль
        /// </summary>
        /// <param name="updatePasswordRequest">Запрос на обновление пароля</param>
        /// <returns></returns>
        [HttpPatch("Password/Update")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequestDTO updatePasswordRequest)
        {
            if (!ModelState.IsValid)
            {
                return base.BadRequestModelState();
            }

            // Определение пользователя
            var user = await _authenticationProvider.GetHttpContextUser(HttpContext.User);
            if (user == null)
            {
                // Если пользователь не найден
                return NotFoundWithErrorOf<string>("User not found!");
            }

            try
            {
                // Обновление пароля
                await _userProvider.UpdatePassword(user, updatePasswordRequest);

                return Ok();
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

        [HttpPatch("Color/Update")]
        [Authorize]
        public async Task<IActionResult> UpdateColor([FromBody] UpdateColorRequestDTO updateColorRequestDTO)
        {
            if (!ModelState.IsValid)
            {
                return base.BadRequestModelState();
            }

            // Определение пользователя
            var user = await _authenticationProvider.GetHttpContextUser(HttpContext.User);
            if (user == null)
            {
                // Если пользователь не найден
                return NotFoundWithErrorOf<string>("User not found!");
            }

            try
            {
                await _userProvider.UpdateColor(user, updateColorRequestDTO.Color);

                return Ok();
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

        [HttpPatch("Name/Update")]
        [Authorize]
        public async Task<IActionResult> UpdateName([FromBody] UpdateNameRequestDTO updateNameRequestDTO)
        {
            if (!ModelState.IsValid)
            {
                return base.BadRequestModelState();
            }

            // Определение пользователя
            var user = await _authenticationProvider.GetHttpContextUser(HttpContext.User);
            if (user == null)
            {
                // Если пользователь не найден
                return NotFoundWithErrorOf<string>("User not found!");
            }

            try
            {
                await _userProvider.UpdateName(user, updateNameRequestDTO.Name);

                return Ok();
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
