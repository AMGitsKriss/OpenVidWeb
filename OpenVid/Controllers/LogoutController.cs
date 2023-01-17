using Database.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenVid.Extensions;
using System.Threading.Tasks;

namespace OpenVid.Controllers
{
    public class LogoutController : OpenVidController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        public LogoutController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(SiteMap.Home);
        }
    }
}
