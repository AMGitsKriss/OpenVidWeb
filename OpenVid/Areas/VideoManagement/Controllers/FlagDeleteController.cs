using Microsoft.AspNetCore.Mvc;
using OpenVid.Extensions;
using VideoHandler;

namespace OpenVid.Areas.VideoManagement.Controllers
{
    [Area("videomanagement")]
    public class FlagDeleteController : OpenVidController
    {
        private IVideoManager _videoManager;

        public FlagDeleteController(IVideoManager videoManager)
        {
            _videoManager = videoManager;
        }

        [HttpPost]
        public IActionResult Index(int id)
        {
            _videoManager.SoftDelete(id);

            return RedirectToAction(SiteMap.Playback_Play, new { Id = id });
        }
    }
}
