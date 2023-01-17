using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OpenVid.Areas.Playback.Models;
using OpenVid.Areas.Playback.Models.Play;
using OpenVid.Extensions;
using OrionDashboard.Web.Attributes;
using VideoHandler;

namespace OpenVid.Areas.Playback.Controllers
{
    [RequireLogin]
    [Area("playback")]
    public class PlayController : OpenVidController
    {

        private IVideoManager _videoService;
        private IUrlResolver _urlResolver;

        public PlayController(IVideoManager videoService, IUrlResolver urlResolver)
        {
            _videoService = videoService;
            _urlResolver = urlResolver;
        }

        [Route("{id:int}")]
        public IActionResult Index(int id)
        {
            var video = _videoService.GetVideo(id);

            if (video == null)
                return NotFound();

            PlayViewModel viewModel = new PlayViewModel()
            {
                Id = id,
                Name = video.Name,
                VideoSources = _urlResolver.GetVideoUrls(video)
            };

            return View(viewModel);
        }

        public IActionResult VideoDetails(int id)
        {
            var video = _videoService.GetVideo(id);

            var tagCollection = video.VideoTag.Select(x => x.Tag.Name).OrderBy(t => t);

            var viewModel = new VideoDetailsViewModel()
            {
                Id = video.Id,
                Name = video.Name,
                Description = video.Description,
                Tags = string.Join(", ", tagCollection),
                IsFlaggedForDeletion = video.IsDeleted,
                Rating = _videoService.GetRatings().FirstOrDefault(r => r.Id == video.RatingId)?.Name ?? "Unrated",
                Metadata = video.VideoSource.Select(s => new MetadataViewModel()
                {
                    Md5 = s.Md5,
                    Extension = s.Extension,
                    Width = s.Width,
                    Height = s.Height,
                    Size = s.Size,
                    DurationInSeconds = (int)video.Length.TotalSeconds
                }).First(s => s.Width > 0)
            };

            return PartialView("_VideoDetails", viewModel);
        }
    }
}
