using Database.Models;
using OpenVid.Models;
using OpenVid.Models.Shared;
using System.Collections.Generic;

namespace OpenVid.Areas.Playback.Models.Search
{
    public class SearchViewModel : BaseViewModel
    {
        public List<TagViewModel> TagGroups { get; set; }
    }
}
