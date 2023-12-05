using AllianzBackEnd.Domain.Enums;
using AllianzBackEnd.Domain;
using Microsoft.AspNetCore.Mvc;

namespace AllianzBackEnd.API.Controllers
{
    public class BaseController : ControllerBase
    {
        //protected readonly ILogger _logger;

        public BaseController()
        {
        }

        protected IActionResult Ok<T>(T result)
        {
            if (result?.GetType() == typeof(ApiResponse<>))
            {
                return base.Ok(result);
            }
            return base.Ok(ApiResponse.Ok(result));
        }

        protected IActionResult Done<T>(ApiResponse<T> response)
        {
            if (response.Status == ResultStatus.Failed ||
                response.Status == ResultStatus.Unknown)
            {
                return BadRequest(response);
            }

            return base.Ok(response);
        }
    }
}
