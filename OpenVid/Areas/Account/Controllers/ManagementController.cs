using Microsoft.AspNetCore.Mvc;
using OpenVid.Extensions;
using OrionDashboard.Web.Attributes;

namespace OpenVid.Areas.Account.Controllers
{
    [RequirePermission(Database.Users.Permissions.Account_Management)]
    [Area("account")]
    public class ManagementController : OpenVidController
    {
        public ManagementController()
        {

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SetAccountConfirmation(string userId, bool setIsConfirmed)
        {
            return View();
        }
    }
}
