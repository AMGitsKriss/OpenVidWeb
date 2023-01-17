using CatalogManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenVid.Areas.Catalog.Models.ImportPreprocessed;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VideoHandler;
using VideoHandler.Models;

namespace OpenVid.Areas.Catalog.Controllers
{
    [Area("catalog")]
    public class ImportPreprocessedController : Controller
    {
        private readonly IVideoManager _save;
        private readonly CatalogImportOptions _configuration;

        public ImportPreprocessedController(IVideoManager save, IOptions<CatalogImportOptions> configuration)
        {
            _save = save;
            _configuration = configuration.Value;
        }

        public IActionResult Index()
        {
            var viewModel = new ImportPreprocessedViewModel()
            {
                DiscoveredFiles = FindFiles()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Save(string fileName)
        {
            SaveVideoResponse response;
            var pendingFiles = FindFiles();

            var fileInfo = pendingFiles.FirstOrDefault(f => f.FileName == fileName);

            if (fileInfo == null)
            {
                response = new SaveVideoResponse()
                {
                    Message = $"The file {fileInfo.FileName} could not longer be found."
                };
                return Json(response);
            }

            try
            {
                ImportVideoRequest request = new ImportVideoRequest()
                {
                    FileName = fileInfo.FileName,
                    FileLocation = fileInfo.FileLocation
                };

                response = await _save.ImportVideoAsync(request);
                if (!response.AlreadyExists)
                {
                    _save.SaveTagsForVideo(response.Video, _save.DefineTags(fileInfo.SuggestedTags));
                }

                if (response.Video != null && !response.AlreadyExists)
                {
                    response.Message = "Success!";
                }
            }
            catch (Exception ex)
            {
                response = new SaveVideoResponse()
                {
                    Message = ex.Message
                };
            }

            return Json(new ImportResultViewModel()
            {
                Message = response.Message
            });
        }

        private List<FoundVideoViewModel> FindFiles()
        {
            var importDir = $@"{_configuration.InternalDirectory}\import_queue";
            Directory.CreateDirectory(importDir);
            return FindFiles(importDir, importDir);
        }

        private List<FoundVideoViewModel> FindFiles(string dir, string prefix)
        {
            try
            {
                var result = new List<FoundVideoViewModel>();

                var suggestedTags = dir.Replace(prefix, "").Split(@"\", StringSplitOptions.RemoveEmptyEntries).ToList();

                // Files
                foreach (var file in Directory.GetFiles(dir))
                {
                    var info = new FileInfo(file);
                    var video = new FoundVideoViewModel()
                    {
                        FileName = info.Name,
                        FullName = info.FullName,
                        FileLocation = info.DirectoryName,
                        SuggestedTags = suggestedTags
                    };
                    result.Add(video);
                }

                // Directories
                foreach (var folder in Directory.GetDirectories(dir))
                {
                    result.AddRange(FindFiles(folder, prefix));
                }

                return result;
            }
            catch (DirectoryNotFoundException)
            {

                return FindFiles(dir, prefix);
            }
        }
    }
}
