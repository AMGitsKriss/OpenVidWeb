using System.Collections.Generic;

namespace OpenVid.Areas.Playback.Models.Shared
{
    public class SuggestedTagViewModel
    {
        public string TagName { get; set; }
        public List<RelatedTagViewModel> RelatedTags { get; set; }
    }
}
