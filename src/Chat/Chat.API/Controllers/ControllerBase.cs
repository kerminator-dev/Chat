using Chat.API.Models.Responses;
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
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));

            return BadRequest(new ErrorResponse(errorMessages));
        }
    }
}
