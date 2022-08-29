using Chat.API.Models.Responses;
using Chat.API.Services.Providers;
using Chat.API.Services.UserRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Chat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationsController : Controllers.ControllerBase
    {
        private readonly ConversationProvider _conversationProvider;
        private readonly IUserRepository _userRepository;

        public ConversationsController(ConversationProvider conversationProvider, IUserRepository userRepository)
        {
            _conversationProvider = conversationProvider;
            _userRepository = userRepository;
        }

        [HttpGet("GetAll")]
        [Authorize]
        public async Task<IActionResult> GetAllUserConverstaions()
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            if (!int.TryParse(HttpContext.User.FindFirstValue("id"), out int userId))
            {
                return NotFound(new ErrorResponse("User not found"));
            }

            var user = await _userRepository.Get(userId);
            if (user == null)
            {
                return NotFound(new ErrorResponse("User not found"));
            }

            var responce =  await _conversationProvider.GetAllUserConversations(user);

            return Ok(responce);
        }
    }
}
