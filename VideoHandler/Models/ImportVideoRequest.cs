using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VideoHandler.Models
{
    public class ImportVideoRequest
    {
        public string FileLocation { get; set; }
        public string FileName { get; set; }
        public string FileNameFull => Path.Combine(FileLocation, FileName);
    }
}
