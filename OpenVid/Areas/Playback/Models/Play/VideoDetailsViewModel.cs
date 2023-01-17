using System.Collections.Generic;

namespace OpenVid.Areas.Playback.Models.Play
{
    public class VideoDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public bool IsFlaggedForDeletion { get; set; }
        public string Rating { get; set; }
        public MetadataViewModel Metadata { get; set; }
    }
}
