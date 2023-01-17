using OpenVid.Models;
using OpenVid.Models.Shared;
using System.Collections.Generic;

namespace OpenVid.Areas.VideoManagement.Models.Curation
{
    public class CurationViewModel : BaseViewModel
    {
        public List<VideoViewModel> VideosForDeletion { get; internal set; }
    }
}
