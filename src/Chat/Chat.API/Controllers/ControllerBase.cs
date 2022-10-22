using Chat.API.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers
{
    public class ControllerBase : Controller
    {
        /// <summary>
        /// Получить список ошибок состояния модели
        /// </summary>
        /// <returns></returns>
        protected BadRequestObjectResult BadRequestModelState()
        {
            var responseBody = new ErrorResponse<string>
            (
                errors: ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
            );

            return BadRequest(responseBody);
        }

        protected ConflictObjectResult ConflictWithErrorOf<TError>(TError errorMessage)
        {
            var responseBody = new ErrorResponse<TError>(errorMessage);

            return Conflict(responseBody);
        }

        protected BadRequestObjectResult BadRequestWithErrorOf<TError>(TError errorMessage)
        {
            var responseBody = new ErrorResponse<TError>(errorMessage);

            return BadRequest(responseBody);
        }

        protected NotFoundObjectResult NotFoundWithErrorOf<TError>(TError errorMessage)
        {
            var responseBody = new ErrorResponse<TError>(errorMessage);

            return NotFound(responseBody);
        }

        protected UnauthorizedObjectResult UnauthorizedWithErrorOf<TError>(TError errorMessage)
        {
            var responseBody = new ErrorResponse<TError>(errorMessage);

            return Unauthorized(responseBody);
        }
    }
}
