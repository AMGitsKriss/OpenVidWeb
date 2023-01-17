using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Database.Models
{
    public partial class VideoEncodeQueue
    {
        public int Id { get; set; }
        public int VideoId { get; set; }
        public string InputDirectory { get; set; }
        public string OutputDirectory { get; set; }
        public string Encoder { get; set; }
        public string RenderSpeed { get; set; }
        public string VideoFormat { get; set; }
        public string PlaybackFormat { get; set; }
        public double Quality { get; set; }
        public int MaxHeight { get; set; }
        public bool IsVertical { get; set; }
        public bool IsDone { get; set; }

        public virtual Video Video { get; set; }
    }
}
