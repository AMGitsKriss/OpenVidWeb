using OpenVid.Models.Shared;
using System.Collections.Generic;

namespace OpenVid.Models.Home
{
    public class HomeViewModel : BaseViewModel
    {
        public List<TagViewModel> TagGroups { get; set; }
    }
}
