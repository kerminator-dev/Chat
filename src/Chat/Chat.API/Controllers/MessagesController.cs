using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Chat.API.DTOs.Requests;
using Chat.API.DTOs.Responses;
using Chat.API.Services.Providers;
using Chat.API.Exceptions;
using Chat.API.DTOs.Responses.TechnicalMessages;

namespace Chat.API.Controllers
{
    [Route("api/Dialogues/[controller]")]
    [ApiController]
    
    public class MessagesController : Controllers.ControllerBase
    {
        private readonly MessageProvider _messageProvider;
        private readonly AuthenticationProvider _authenticationProvider;

        public MessagesController(MessageProvider messageProvider, AuthenticationProvider authenticationProvider)
        {
            _messageProvider = messageProvider;
            _authenticationProvider = authenticationProvider;
        }

        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <param name="message">Запрос на отправку сообщения</param>
        /// <returns></returns>
        [HttpPost("Send")]
        [Authorize]
        public async Task<IActionResult> SendMessages([FromBody] SendMessageRequestDTO message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            // Определение пользователя
            var messageSender = await _authenticationProvider.GetHttpContextUser(HttpContext.User);
            if (messageSender == null)
            {
                // Если пользователь не найден
                return NotFound(new ErrorResponseDTO("User not found!"));
            }

            try
            {
                // Отправка сообщения
                await _messageProvider.SendMessage(messageSender, message);
            }
            catch (ProcessingException ex)
            {
                return BadRequest(new ErrorResponseDTO(ex.Message));
            }
            catch (Exception) { }

            return Ok();
        }

        /// <summary>
        /// Получить сообщения
        /// </summary>
        /// <param name="getMessagesRequest"></param>
        /// <returns></returns>
        [HttpPost("Get")]
        [Authorize]
        public async Task<IActionResult> GetMessages([FromBody] GetMessagesRequestDTO getMessagesRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            // Определение пользователя
            var user = await _authenticationProvider.GetHttpContextUser(HttpContext.User);
            if (user == null)
            {
                // Если пользователь не найден
                return NotFound(new ErrorResponseDTO("User not found!"));
            }

            try
            {
                // Получение сообщений
                var messagesResponse = await _messageProvider.GetMessages(user, getMessagesRequest);

                return Ok(messagesResponse);
            }
            catch (ProcessingException ex)
            {
                return BadRequest(new ErrorResponseDTO(ex.Message));
            }
        }

        /// <summary>
        /// Удалить сообщения
        /// </summary>
        /// <param name="deleteMessageRequest">Запрос на удаление сообщений</param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        [Authorize]
        public async Task<IActionResult> DeleteMessages([FromBody] DeleteMessagesRequestDTO deleteMessageRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            // Определение пользователя
            var user = await _authenticationProvider.GetHttpContextUser(HttpContext.User);
            if (user == null)
            {
                // Если пользователь не найден
                return NotFound(new ErrorResponseDTO("User not found!"));
            }

            try
            {
                // Удаление сообщения
                await _messageProvider.DeleteMessages(user, deleteMessageRequest);

                return Ok();
            }
            catch (ProcessingException ex)
            {
                return BadRequest(new ErrorResponseDTO(ex.Message));
            }
        }

        /// <summary>
        /// Изменить/обновить сообщение
        /// </summary>
        /// <param name="updateMessageRequest">Запрос на изменение сообщения</param>
        /// <returns></returns>
        [HttpPatch("Update")]
        [Authorize]
        public async Task<IActionResult> UpdateMessage([FromBody] UpdateMessageRequestDTO updateMessageRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            // Определение пользователя
            var user = await _authenticationProvider.GetHttpContextUser(HttpContext.User);
            if (user == null)
            {
                // Если пользователь не найден
                return NotFound(new ErrorResponseDTO("User not found!"));
            }

            try
            {
                // Обновление сообщения
                await _messageProvider.UpdateMessage(user, updateMessageRequest);

                return Ok();
            }
            catch (ProcessingException ex)
            {
                return BadRequest(new ErrorResponseDTO(ex.Message));
            }
        }
    }
}
