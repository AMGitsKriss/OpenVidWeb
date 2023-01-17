using Database.Models;

namespace VideoHandler.Models
{
    public class SaveVideoResponse
    {
        public Video Video { get; set; }
        public bool AlreadyExists { get; set; }
        public string Message { get; set; }
    }
}
