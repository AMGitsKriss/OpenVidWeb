using System.Collections.Generic;

namespace CatalogManager.Models
{
    public class FoundVideo
    {
        public string FileName { get; set; }
        public string FullName { get; set; }
        public string FileLocation { get; set; }
        public string Resolution { get; set; }
        public bool IsVertical { get; set; }
        public List<string> SuggestedTags { get; set; }
        public long Size { get; set; }
        public string PlaybackFormat { get; set; }
    }
}
