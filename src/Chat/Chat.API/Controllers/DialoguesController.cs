using Chat.API.DTOs.Requests;
using Chat.API.Entities;
using Chat.API.Exceptions;
using Chat.API.Services.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DialoguesController : Controllers.ControllerBase
    {
        private readonly AuthenticationProvider _authenticationProvider;
        private readonly DialogueProvider _dialogueProvider;

        public DialoguesController(AuthenticationProvider authenticationProvider, DialogueProvider dialogueProvider)
        {
            _authenticationProvider = authenticationProvider;
            _dialogueProvider = dialogueProvider;
        }

        /// <summary>
        /// Получить список всех диалогов для пользователя из контекста
        /// </summary>
        /// <returns></returns>
        [HttpGet("All")]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            // Определение пользователя
            User user = await _authenticationProvider.GetHttpContextUser(HttpContext.User);
            if (user == null)
            {
                // Если такого пользователя нет
                return NotFoundWithErrorOf<string>("User not found");
            }

            try
            {
                // Получение списка диалогов пользователя
                var userDialoguesResponce = await _dialogueProvider.GetAll(user);

                return Ok(userDialoguesResponce);
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
        /// Создать диалог
        /// </summary>
        /// <param name="createDialogueRequest">Запрос на создание диалога</param>
        /// <returns></returns>
        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateDialogueRequestDTO createDialogueRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            // Определение пользователя (инициатора диалога)
            var requester = await _authenticationProvider.GetHttpContextUser(HttpContext.User);
            if (requester == null)
            {
                // Если такого пользователя нет
                return NotFoundWithErrorOf<string>("User not found");
            }

            // Поиск второго участника диалога
            var targetUser = await _authenticationProvider.GetUser(createDialogueRequest.TargetUserId);
            if (targetUser == null)
            {
                // Если такого пользователя нет
                return NotFoundWithErrorOf<string>("User not found");
            }

            // Если пользователь пытается создать диалог сам с собой
            if (requester.Id == targetUser.Id)
            {
                return BadRequestWithErrorOf("Cannot create dialog with yourself! (may be later)");
            }

            try
            {
                // Создание диалога
                await _dialogueProvider.Create(requester, targetUser);

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

        /// <summary>
        /// Удалить диалог
        /// </summary>
        /// <param name="deleteDialogueRequest">Запрос на удаление диалога</param>
        /// <returns></returns>
        [HttpDelete("Delete/{dialogueId}")]
        [Authorize]
        public async Task<IActionResult> Delete(int dialogueId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            // Определение пользователя
            var requester = await _authenticationProvider.GetHttpContextUser(HttpContext.User);
            if (requester == null)
            {
                // Если такого пользователя нет
                return NotFoundWithErrorOf<string>("User not found");
            }

            try
            {
                // Удаление диалога
                await _dialogueProvider.Delete(requester, dialogueId);

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
