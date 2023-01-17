using OpenVid.Models;
using OpenVid.Models.Shared;
using System.Collections.Generic;

namespace OpenVid.Areas.Playback.Models.VideoList
{
    public class VideoListViewModel : BaseViewModel
    {
        public List<VideoViewModel> Videos { get; set; }
        public string SearchQuery { get; set; }
        public bool HasNextPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
