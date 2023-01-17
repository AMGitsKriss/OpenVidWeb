using CatalogManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OrionDashboard.Web.Attributes;
using System.IO;
using System.Threading.Tasks;

namespace OpenVid.Areas.Playback.Controllers
{
    [RequireLogin]
    [Area("playback")]
    public class ThumbnailController : Controller
    {
        private readonly CatalogImportOptions _configuration;
        private readonly ThumbnailService _playbackService;

        public ThumbnailController(IOptions<CatalogImportOptions> configuration, ThumbnailService playbackService)
        {
            _configuration = configuration.Value;
            _playbackService = playbackService;
        }

        [Route("[area]/[controller]/{id}")]
        public async Task<IActionResult> Index([FromRoute] int id)
        {
            var thumbnail = GetManualThumbnail(id) ?? await GetAutoThumbnail(id) ?? GetDefaultThumbnail();

            return File(thumbnail, "image/jpeg");
        }

        public async Task<IActionResult> Auto(int id)
        {
            return File(await GetAutoThumbnail(id) ?? GetDefaultThumbnail(), "image/jpeg");
        }

        public IActionResult Manual(int id)
        {
            return File(GetManualThumbnail(id) ?? GetDefaultThumbnail(), "image/jpeg");
        }

        private byte[] GetManualThumbnail(int id)
        {
            var idString = id.ToString().PadLeft(2, '0');
            var thumbnailFolder = Path.Combine(_configuration.BucketDirectory, "thumbnail", idString.Substring(0, 2));
            var manualThumbnailFile = $"m{idString}.jpg";

            // Check if we've manually assigned one
            var manualThumbnailFullName = Path.Combine(thumbnailFolder, manualThumbnailFile);
            if (!System.IO.File.Exists(manualThumbnailFullName))
            {
                return null;
            }

            return System.IO.File.ReadAllBytes(manualThumbnailFullName);
        }

        private async Task<byte[]> GetAutoThumbnail(int id)
        {
            var idString = id.ToString().PadLeft(2, '0');
            var thumbnailFolder = Path.Combine(_configuration.BucketDirectory, "thumbnail", idString.Substring(0, 2));
            var autoThumbnailFile = $"{idString}.jpg";

            var autoThumbnailFullName = Path.Combine(thumbnailFolder, autoThumbnailFile);
            if (!System.IO.File.Exists(autoThumbnailFullName))
            {
                await _playbackService.TryGenerateThumbnail(id);
            }

            // Otherwise just return the placeholder card
            if (!System.IO.File.Exists(autoThumbnailFullName))
            {
                return null;
            }

            return System.IO.File.ReadAllBytes(Path.Combine(thumbnailFolder, autoThumbnailFullName));
        }

        private byte[] GetDefaultThumbnail()
        {
            return System.IO.File.ReadAllBytes("wwwroot/img/thumb.png");
        }
    }
}
