using Chat.API.DTOs.Responses;
using Chat.API.DTOs.Responses.TechnicalMessages;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers
{
    public class ControllerBase : Controller
    {
        /// <summary>
        /// Получить список ошибок состояния модели
        /// </summary>
        /// <returns></returns>
        protected IActionResult BadRequestModelState()
        {
            // Получение списка ошибок модели
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));

            return BadRequest(new ErrorResponseDTO(errorMessages));
        }
    }
}
