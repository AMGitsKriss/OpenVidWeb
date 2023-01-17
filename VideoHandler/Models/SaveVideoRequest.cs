using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace VideoHandler.Models
{
    public class SaveVideoRequest
    {
        public IFormFile File { get; set; }
    }
}
