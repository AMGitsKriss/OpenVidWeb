using Database.Models;
using System.Collections.Generic;

namespace OpenVid.Models.Shared
{
    public class TagViewModel
    {
        public string Category { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
