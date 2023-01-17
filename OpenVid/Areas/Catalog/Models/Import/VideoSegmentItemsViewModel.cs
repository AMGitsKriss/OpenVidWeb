using Database.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace OpenVid.Areas.Catalog.Models.Import
{
    public class VideoSegmentItemsViewModel
    {
        public int VideoId { get; set; }
        public string VideoName { get; set; }
        public List<VideoSegmentQueueItem> Segments { get; set; }
    }

    public class UploadSubtitleRequest
    {
        public int VideoId { get; set; }
        public string Language { get; set; }
        public IFormFile Subtitle { get; set; }
    }
}
