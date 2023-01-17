using VideoHandler;

namespace VideoHandler.Models
{
    public class SearchParameter
    {
        public FilterType Type { get; set; }
        public string Value { get; set; }
        public bool InvertSearch { get; set; }
    }
}
