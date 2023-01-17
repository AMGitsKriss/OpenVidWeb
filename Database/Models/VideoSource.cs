using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Database.Models
{
    public partial class VideoSource
    {
        public int Id { get; set; }
        public int VideoId { get; set; }
        public string Md5 { get; set; }
        public string Extension { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public long Size { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Video Video { get; set; }
    }
}
