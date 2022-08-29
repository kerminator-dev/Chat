using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Chat.API.Models.Requests;
using Chat.API.Models.Responses;
using Chat.API.Services.Providers;
using Chat.API.Services.DialogueRepositories;
using System.Reflection;

namespace Chat.API.Controllers
{
    [Route("api/[controller]")]
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

        [HttpPost("Send")]
        [Authorize]
        public async Task<IActionResult> Send(SendMessageRequest message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            var messageSender = await _authenticationProvider.GetHttpContextUser(HttpContext.User);
            if (messageSender == null)
            {
                return NotFound(new ErrorResponse("User not found!"));
            }

            await _messageProvider.SendMessage(messageSender, message);

            return Ok();
        }

        [HttpPost("Get")]
        [Authorize]
        public async Task<IActionResult> GetMessages(GetMessagesRequest getMessagesRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            var user = await _authenticationProvider.GetHttpContextUser(HttpContext.User);
            if (user == null)
            {
                return NotFound(new ErrorResponse("User not found!"));
            }

            var messagesResponse = await _messageProvider.GetMessages(user, getMessagesRequest);
            if (messagesResponse == null || messagesResponse.Messages == null)
            {
                return NotFound(new ErrorResponse("Messages not found!"));
            }
            
            return Ok(messagesResponse);
        }
    }
}
