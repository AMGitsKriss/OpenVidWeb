using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Database.Models
{
    public partial class VideoSegmentQueue
    {
        public int Id { get; set; }
        public int VideoId { get; set; }
        public bool IsReady { get; set; }
        public bool IsDone { get; set; }

        public virtual Video Video { get; set; }
        public virtual List<VideoSegmentQueueItem> VideoSegmentQueueItem { get; set; }
    }
}
