namespace CatalogManager.Models
{
    public class SubtitleFile
    {
        public string StreamId { get; internal set; }
        public string OutputFileName { get; internal set; }
        public string SourceFileFullName { get; internal set; }
        public string Language { get; internal set; }
        public string OriginalFormat { get; set; }
    }
}
