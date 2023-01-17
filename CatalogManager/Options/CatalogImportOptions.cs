using System.Collections.Generic;

namespace CatalogManager
{
    public class CatalogImportOptions
    {
        public string ImportDirectory { get; set; }
        public List<EncoderPresetOptions> EncoderPresets { get; set; }

        public string BucketUrl { get; set; }
        public string BucketDirectory { get; set; }
        public string InternalUrl { get; set; }
        public string InternalDirectory { get; set; }
        public int ThumbnailFramesIntoVideo { get; set; } = 60;
    }
}
