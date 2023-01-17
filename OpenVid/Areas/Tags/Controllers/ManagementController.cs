using Microsoft.AspNetCore.Mvc;

namespace OpenVid.Areas.Tags.Controllers
{
    [Area("tags")]
    public class ManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
