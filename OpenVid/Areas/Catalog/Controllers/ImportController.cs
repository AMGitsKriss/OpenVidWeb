using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CatalogManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenVid.Areas.Catalog.Models.Import;
using OpenVid.Models;
using VideoHandler;

namespace OpenVid.Areas.Catalog.Controllers
{
    [Area("catalog")]
    public class ImportController : Controller
    {
        private readonly ImportService _importService;
        private readonly IVideoManager _videoManager;

        public ImportController(ImportService importService, IVideoManager videoManager)
        {
            _importService = importService;
            _videoManager = videoManager;
        }

        public IActionResult Index()
        {
            var model = new ImportViewModel()
            {
                FilesPendingQueueing = _importService.FindFiles(),
                FilesPendingEncode = _importService.GetQueuedFiles(),
                FilesPendingSegmenting = _importService.GetPendingSegmenting()
            };
            return View(model);
        }

        /// <summary>
        /// Upload one or more files into the /wwwroot/import/01_pending/ directory.
        /// When done, we'll replace the first pannel.
        /// </summary>
        [HttpPost]
        [RequestSizeLimit(10L * 1024L * 1024L * 1024L)]
        [RequestFormLimits(MultipartBodyLengthLimit = 10L * 1024L * 1024L * 1024L)]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var success = await _importService.UploadFile(file);

            if (success)
                return Ok();

            return StatusCode(500);
        }

        /// <summary>
        /// Scan the contents of the /wwwroot/import/01_pending/ directory. 
        /// For each encoder preset, make a copy in /wwwroot/import/02_queued/ and add a job to the database. 
        /// </summary>
        public IActionResult Queue()
        {
            _importService.IngestFiles();
            return Json(new object());
        }

        public IActionResult VideoSegmentModal(int id)
        {
            var video = _videoManager.GetVideo(id);
            var model = new VideoSegmentItemsViewModel()
            {
                VideoId = video.Id,
                VideoName = video.Name,
                Segments = video.VideoSegmentQueueItem.ToList()
            };
            return PartialView("_VideoSegmentModal", model);
        }

        [HttpPost]
        public AjaxResponse SaveSubtitles(UploadSubtitleRequest request)
        {
            try
            {
                if (!request.Subtitle.ContentType.Contains("application/octet-stream") || request.Subtitle.Length == 0)
                    throw new Exception("Incorrect file format");

                using (var fileStream = new MemoryStream())
                {
                    request.Subtitle.CopyTo(fileStream);
                    _importService.SaveSubtitle(request.VideoId, request.Language, request.Subtitle.FileName, fileStream.ToArray());
                }

                return new AjaxResponse();
            }
            catch (Exception ex)
            {
                return new AjaxResponse()
                {
                    Message = ex.Message + " Are you sure this is a jpeg?"
                };
            }
        }
    }
}
