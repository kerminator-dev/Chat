using Chat.API.Exceptions;
using Chat.API.Models.Requests;
using Chat.API.Models.Responses;
using Chat.API.Services.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AuthenticationProvider _authenticationProvider;
        private readonly UserProvider _userProvider;

        public UsersController(AuthenticationProvider authenticationProvider, UserProvider userProvider)
        {
            _authenticationProvider = authenticationProvider;
            _userProvider = userProvider;
        }

        [HttpPost("Search")]
        [Authorize]
        public async Task<IActionResult> SearchUser([FromBody] SearchUserRequest searchUserRequest)
        {
            if (!ModelState.IsValid)
            {
                return base.BadRequestModelState();
            }

            var user = _authenticationProvider.GetHttpContextUser(HttpContext.User);
            if (user == null)
            {
                return NotFound(new ErrorResponse("User not found!"));
            }

            try
            {
                var response = await _userProvider.Search(searchUserRequest);

                return Ok(response);
            }
            catch (ProcessingException ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }
        }

        [HttpPost("Get")]
        [Authorize]
        public async Task<IActionResult> GetUsers([FromBody] GetUsersRequest getUsersRequest)
        {
            if (!ModelState.IsValid)
            {
                return base.BadRequestModelState();
            }

            var user = _authenticationProvider.GetHttpContextUser(HttpContext.User);
            if (user == null)
            {
                return NotFound(new ErrorResponse("User not found!"));
            }
            try
            {
                var response = await _userProvider.Get(getUsersRequest);
                return Ok(response);
            }
            catch (ProcessingException ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }
        }
    }
}
