using Database;
using Database.Models;
using System.Collections.Generic;
using System.Linq;
using VideoHandler.Attributes;
using VideoHandler.Models;

namespace VideoHandler.SearchFilters
{
    [Filter(FilterType.General)]
    public class GeneralFilter : IFilter
    {
        private IVideoRepository _repo;
        public GeneralFilter(IVideoRepository repo)
        {
            _repo = repo;
        }

        public List<Video> Filter(SearchParameter parameter)
        {
            return FindByName(parameter.Value, parameter.InvertSearch);
        }

        public List<Video> FindByName(string tag, bool invert)
        {
            List<Video> result = _repo.GetViewableVideos().Where(v => v.Name.Contains(tag) == !invert).ToList();

            return result;

        }
    }
}
