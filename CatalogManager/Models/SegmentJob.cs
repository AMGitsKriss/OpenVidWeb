namespace CatalogManager.Models
{
    public class SegmentJob
    {
        public int VideoId { get; set; }
        public int JobId { get; set; }
        public string Name { get; set; }
        public bool IsReady { get; set; }
    }
}
