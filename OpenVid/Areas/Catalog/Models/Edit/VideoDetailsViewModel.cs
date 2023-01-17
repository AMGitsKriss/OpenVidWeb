using Database.Models;
using OpenVid.Areas.Playback.Models;
using OpenVid.Areas.Playback.Models.Shared;
using System.Collections.Generic;

namespace OpenVid.Areas.Catalog.Models.Edit
{
    public class VideoDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public bool IsFlaggedForDeletion { get; set; }
        public int RatingId { get; set; }
        public List<MetadataViewModel> Metadata { get; set; }
        public List<Ratings> PossibleRatings { get; set; }
        public List<SuggestedTagViewModel> SuggestedTags { get; set; }
    }
    public class VideoThumbnailsViewModel
    {
        public int Id { get; set; }
    }
}
