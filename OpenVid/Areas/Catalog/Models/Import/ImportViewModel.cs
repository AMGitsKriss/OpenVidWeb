using CatalogManager.Models;
using System.Collections.Generic;

namespace OpenVid.Areas.Catalog.Models.Import
{
    public class ImportViewModel
    {
        public List<FoundVideo> FilesPendingQueueing { get; set; }
        public List<FoundVideo> FilesPendingEncode { get; set; }
        public List<SegmentJob> FilesPendingSegmenting { get; set; }
    }
}
