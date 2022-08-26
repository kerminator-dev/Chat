using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Chat.API.Services.Messangers;
using Chat.API.Services.Authenticators;
using Chat.API.Models.Requests;
using Chat.API.Models.Responses;

namespace Chat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class MessagesController : ControllerBase
    {
        private readonly MessageProvider _messageProvider;
        private readonly AuthenticationProvider _authenticationProvider;

        public MessagesController(MessageProvider messageProvider, AuthenticationProvider authenticationProvider)
        {
            _messageProvider = messageProvider;
            _authenticationProvider = authenticationProvider;
        }

        /// <summary>
        /// Можно сделать реализацию отправки сообщений и через SignalR Hub,
        /// Но при отсутствующем подключении к хабу не получится отправить сообщение 
        /// И хотя бы внести его в БД
        /// Поэтому отправка сообщения реализована через API-метод
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost("Send")]
        [Authorize]
        public async Task<IActionResult> Send(SendMessageRequest message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            if (!int.TryParse(HttpContext.User.FindFirstValue("id"), out int userId))
            {
                return NotFound(new ErrorResponse("User not found"));
            }

            var messageSender = await _authenticationProvider.GetUser(userId);
            if (messageSender == null)
            {
                return NotFound(new ErrorResponse("User not found"));
            }

            var messageReceiver = await _authenticationProvider.GetUser(message.ReceiverId);
            if (messageReceiver == null)
            {
                return NotFound(new ErrorResponse("Receiver not found"));
            }

            await _messageProvider.SendMessage(messageSender, messageReceiver, message);

            return Ok();
        }

        [HttpPost("Get")]
        private async Task<IActionResult> GetMessages()
        {
            return Ok();
        }


        // Переделать
        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));

            return BadRequest(new ErrorResponse(errorMessages));
        }
    }
}
