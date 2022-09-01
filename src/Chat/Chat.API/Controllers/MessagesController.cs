using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Chat.API.Models.Requests;
using Chat.API.Models.Responses;
using Chat.API.Services.Providers;
using Chat.API.Exceptions;

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

        [HttpPost("Send")]
        [Authorize]
        public async Task<IActionResult> SendMessages([FromBody] SendMessageRequest message)
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

            try
            {
                await _messageProvider.SendMessage(messageSender, message);
            }
            catch (ProcessingException ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }
            catch (Exception) { }

            return Ok();
        }

        [HttpPost("Get")]
        [Authorize]
        public async Task<IActionResult> GetMessages([FromBody] GetMessagesRequest getMessagesRequest)
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

            try
            {
                var messagesResponse = await _messageProvider.GetMessages(user, getMessagesRequest);

                return Ok(messagesResponse);
            }
            catch (ProcessingException ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }
            catch (Exception) { }

            return Ok();
        }

        [HttpDelete("Delete")]
        [Authorize]
        public async Task<IActionResult> DeleteMessages([FromBody] DeleteMessagesRequest deleteMessageRequest)
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

            try
            {
                await _messageProvider.DeleteMessage(user, deleteMessageRequest);
            }
            catch (ProcessingException ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }
            catch (Exception) { }

            return Ok();
        }

        [HttpPatch("Update")]
        [Authorize]
        public async Task<IActionResult> UpdateMessage([FromBody] UpdateMessageRequest updateMessageRequest)
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

            try
            {
                await _messageProvider.UpdateMessage(user, updateMessageRequest);
            }
            catch (ProcessingException ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }
            catch (Exception) { }

            return Ok();
        }
    }
}
