using Microsoft.AspNetCore.Mvc;
using OpenVid.Extensions;
using OrionDashboard.Web.Attributes;

namespace OpenVid.Areas.Account.Controllers
{
    [RequireLogin(forceLogin:true)]
    [Area("account")]
    public class MyAccountController : OpenVidController
    {
        public MyAccountController()
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
