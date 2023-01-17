using Database;
using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using VideoHandler.Attributes;
using VideoHandler.Models;

namespace VideoHandler.SearchFilters
{
    [Filter(FilterType.MaxDuration)]
    public class MaxDurationFilter : IFilter
    {
        private IVideoRepository _repo;
        public MaxDurationFilter(IVideoRepository repo)
        {
            _repo = repo;
        }

        public List<Video> Filter(SearchParameter parameter)
        {
            var duration = TimeSpan.Parse(parameter.Value);
            var results = _repo.GetViewableVideos().Where(x => x.Length <= duration).ToList();
            return results;
        }
    }
}
