using Chat.API.DTOs.Requests;
using Chat.API.Exceptions;
using Chat.API.Services.Providers;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Найти пользователя
        /// </summary>
        /// <param name="searchUserRequest">Запрос на поиск пользователей</param>
        /// <returns></returns>
        [HttpPost("Search")]
        [Authorize]
        public async Task<IActionResult> SearchUser([FromBody] SearchUserRequestDTO searchUserRequest)
        {
            if (!ModelState.IsValid)
            {
                return base.BadRequestModelState();
            }

            // Определение пользователя из контекста
            var user = _authenticationProvider.GetHttpContextUser(HttpContext.User);
            if (user == null)
            {
                // Если не найден
                return NotFoundWithErrorOf<string>("User not found!");
            }

            try
            {
                // Поиск пользователей
                var response = await _userProvider.SearchUsers(searchUserRequest);

                return Ok(response);
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
        /// Получить список пользователей по ID
        /// </summary>
        /// <param name="getUsersRequest">Запрос на получение списка пользователей</param>
        /// <returns></returns>
        [HttpPost("Get")]
        [Authorize]
        public async Task<IActionResult> GetUsers([FromBody] GetUsersRequestDTO getUsersRequest)
        {
            if (!ModelState.IsValid)
            {
                return base.BadRequestModelState();
            }

            // Определение пользователя из контекста
            var user = _authenticationProvider.GetHttpContextUser(HttpContext.User);
            if (user == null)
            {
                // Если не найден
                return NotFoundWithErrorOf<string>("User not found!");
            }

            try
            {
                // Получение пользователей
                var response = await _userProvider.GetUsers(getUsersRequest);

                return Ok(response);
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
