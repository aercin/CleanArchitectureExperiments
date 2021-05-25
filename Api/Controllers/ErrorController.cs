using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ErrorController(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("[action]")]
        public IActionResult HandleError()
        { 
            return Ok(HandleErrorResult.GenerateErrorObject($"Hata ile karşılaşıldı. {this._httpContextAccessor.HttpContext.Request.Headers["TrackId"]} numarası ile destek alabilirsiniz")); 
        }

        private class HandleErrorResult
        {
            public HandleErrorResult(string errors)
            {
                Succeeded = false;
                Errors = errors;
            }

            public bool Succeeded { get; private set; }
            public string Errors { get; private set; }

            public static HandleErrorResult GenerateErrorObject(string errors)
            {
                return new HandleErrorResult(errors);
            }
        }
    }
}
