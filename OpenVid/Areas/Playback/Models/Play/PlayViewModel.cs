using OpenVid.Models;
using System.Collections.Generic;

namespace OpenVid.Areas.Playback.Models.Play
{
    public class PlayViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> VideoSources { get; set; }
    }
}
