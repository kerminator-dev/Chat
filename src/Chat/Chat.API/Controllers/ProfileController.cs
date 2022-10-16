﻿using Chat.API.Exceptions;
using Chat.API.Models.Requests;
using Chat.API.Models.Responses;
using Chat.API.Services.Providers;
using Chat.API.Services.UserRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest updatePasswordRequest)
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
                return NotFound(new ErrorResponse("User not found!"));
            }

            try
            {
                // Обновление пароля
                await _userProvider.UpdatePassword(user, updatePasswordRequest);

                return Ok();
            }
            catch (ProcessingException ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }
        }
    }
}
