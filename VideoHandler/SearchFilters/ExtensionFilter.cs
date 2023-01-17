using Database;
using Database.Models;
using System.Collections.Generic;
using System.Linq;
using VideoHandler.Attributes;
using VideoHandler.Models;

namespace VideoHandler.SearchFilters
{
    [Filter(FilterType.Extension)]
    public class ExtensionFilter : IFilter
    {
        private IVideoRepository _repo;
        public ExtensionFilter(IVideoRepository repo)
        {
            _repo = repo;
        }

        public List<Video> Filter(SearchParameter parameter)
        {
            return Extension(parameter.Value, parameter.InvertSearch);
        }

        public List<Video> Extension(string extension, bool invert)
        {
            extension = extension.ToLower();

            List<Video> result;
            if (invert)
            {
                result = _repo.GetViewableVideos().Where(x => !(x.VideoSource.Any(s => s.Extension.ToLower() == extension))).ToList();
            }
            else
            {
                result = _repo.GetViewableVideos().Where(x => x.VideoSource.Any(s => s.Extension.ToLower() == extension)).ToList();
            }
            return result.ToList();

        }
    }
}
