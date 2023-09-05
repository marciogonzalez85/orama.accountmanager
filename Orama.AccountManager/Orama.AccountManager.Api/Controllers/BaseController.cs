using Microsoft.AspNetCore.Mvc;
using Orama.AccountManager.CrossCutting.Common;

namespace Orama.AccountManager.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        public static IActionResult HandlerResponse<T>(Result<T> r)
        where T : class
        {
            return r.StatusCode switch
            {
                200 => new ObjectResult(r),
                201 => new ObjectResult(r) { StatusCode = r.StatusCode },
                202 => new ObjectResult(r) { StatusCode = r.StatusCode },
                204 => new ObjectResult(r) { StatusCode = r.StatusCode },
                400 => new ObjectResult(r) { StatusCode = r.StatusCode },
                404 => new ObjectResult(r) { StatusCode = r.StatusCode },
                500 => new ObjectResult(r) { StatusCode = r.StatusCode },
                _ => throw new ArgumentException($"HandlerStatus invalido {r.StatusCode}")
            };
        }
    }
}
