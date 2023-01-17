using OpenVid.Models;
using System.Collections.Generic;

namespace OpenVid.Areas.Catalog.Models.Edit
{
    public class EditViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> VideoSources { get; set; }
        public VideoDetailsViewModel Update { get; set; }
    }
}
