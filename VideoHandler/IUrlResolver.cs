using Database.Models;
using System.Collections.Generic;

namespace VideoHandler
{
    public interface IUrlResolver
    {
        Dictionary<string, string> GetVideoUrls(Video video);
    }
}