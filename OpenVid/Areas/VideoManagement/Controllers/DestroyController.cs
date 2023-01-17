using Microsoft.AspNetCore.Mvc;
using OpenVid.Extensions;
using OpenVid.Models;
using System;
using VideoHandler;

namespace OpenVid.Areas.VideoManagement.Controllers
{

    [Area("videomanagement")]
    public class DestroyController : OpenVidController
    {
        private IVideoManager _videoManager;

        public DestroyController(IVideoManager videoManager)
        {
            _videoManager = videoManager;
        }

        [HttpPost]
        public IActionResult Index(int id)
        {
            try
            {
                _videoManager.HardDeleteVideo(id);
                return Json(new AjaxResponse());
            }
            catch (Exception ex)
            {
                var response = new AjaxResponse()
                {
                    Message = ex.Message
                };
                return Json(response);
            }
        }
    }
}
