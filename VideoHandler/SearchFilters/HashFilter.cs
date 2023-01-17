using Database;
using Database.Models;
using System.Collections.Generic;
using System.Linq;
using VideoHandler.Attributes;
using VideoHandler.Models;

namespace VideoHandler.SearchFilters
{
    [Filter(FilterType.Hash)]
    public class HashFilter : IFilter
    {
        private IVideoRepository _repo;
        public HashFilter(IVideoRepository repo)
        {
            _repo = repo;
        }

        public List<Video> Filter(SearchParameter parameter)
        {
            return GetFromHash(parameter.Value);
        }

        public List<Video> GetFromHash(string hash)
        {
            List<Video> result = _repo.GetViewableVideos().Where(x => x.VideoSource.Any(s => s.Md5.ToLower() == hash.ToLower())).ToList();
            return result.ToList();

        }
    }
}
