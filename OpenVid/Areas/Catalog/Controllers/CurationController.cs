using Microsoft.AspNetCore.Mvc;
using System.Linq;
using OpenVid.Models.Shared;
using VideoHandler;
using OpenVid.Areas.VideoManagement.Models.Curation;
using OpenVid.Extensions;

namespace OpenVid.Areas.Catalog.Controllers
{
    [Area("catalog")]
    public class CurationController : OpenVidController
    {
        private IVideoManager _videoManager;
        private IUrlResolver _urlResolver;

        public CurationController(IVideoManager videoManager, IUrlResolver urlResolver)
        {
            _videoManager = videoManager;
            _urlResolver = urlResolver;
        }
        public IActionResult Index()
        {
            var model = new CurationViewModel()
            {
                VideosForDeletion = _videoManager.GetSoftDeletedVideos().Select(v => new VideoViewModel()
                {
                    Id = v.Id,
                    Name = v.Name,
                    SizeMb = (int)(v.VideoSource.Sum(s => s.Size) / 1024 / 1024),
                    Length = v.Length.ToString(),
                }).ToList()
            };

            return View(model);
        }
    }
}
