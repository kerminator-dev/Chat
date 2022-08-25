using Chat.API.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Chat.API.Models.Responses;
using Chat.API.Services.Messangers;
using Chat.API.Services.Authenticators;

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

        [HttpPost("Send")]
        [Authorize]
        public async Task<IActionResult> Send(SendMessageRequest message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            var userId = HttpContext.User.FindFirstValue("id");
            if (userId == null)
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

        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));

            return BadRequest(new ErrorResponse(errorMessages));
        }
    }
}
