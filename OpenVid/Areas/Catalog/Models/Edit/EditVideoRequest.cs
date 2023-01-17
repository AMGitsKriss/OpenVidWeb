using Microsoft.AspNetCore.Http;

namespace OpenVid.Areas.Catalog.Models.Edit
{
    public class EditVideoRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public int RatingId { get; set; }
    }

    public class UploadThumbnailRequest
    {
        public int Id { get; set; }
        public IFormFile Image { get; set; }
    }
}
