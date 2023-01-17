using Microsoft.AspNetCore.Mvc;
using OpenVid.Extensions;

namespace OpenVid.Controllers
{
    public class ErrorController : OpenVidController
    {
        [Route("error/{id}")]
        public IActionResult Index(int? statusCode = null)
        {
            if (statusCode.HasValue)
            {
                if (statusCode == 404 || statusCode == 500)
                {
                    var viewName = statusCode.ToString();
                    return View($"~/Views/Shared/Errors/{viewName}.cshtml");
                }
            }

            return View("~/Views/Shared/Errors/404.cshtml");
        }
    }
}
