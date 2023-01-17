namespace OpenVid.Areas.Playback.Models
{
    public class MetadataViewModel
    {
        public string Md5 { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int DurationInSeconds { get; set; }
        public decimal Size { get; set; }
        public string Extension { get; set; }
    }
}