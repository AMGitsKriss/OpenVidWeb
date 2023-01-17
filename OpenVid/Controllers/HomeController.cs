using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OpenVid.Models.Home;
using OrionDashboard.Web.Attributes;
using VideoHandler;

namespace OpenVid.Controllers
{
    [RequireLogin]
    public class HomeController : Controller
    {
        private IVideoManager _manager;

        public HomeController(IVideoManager manager)
        {
            _manager = manager;
        }

        public IActionResult Index()
        {
            var allTags = _manager.GetAllTags().GroupBy(t => t.Type).OrderBy(t => (t.Key ?? 0) );
            HomeViewModel viewModel = new HomeViewModel()
            {
                TagGroups = allTags.Select(t => new Models.Shared.TagViewModel() { 
                    Category = t.FirstOrDefault()?.TypeNavigation?.Name ?? "Tags",
                    Tags = t.ToList()
                }).ToList()
            };

            return View(viewModel);
        }
    }
}
