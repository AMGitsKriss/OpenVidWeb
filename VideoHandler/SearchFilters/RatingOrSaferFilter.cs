using Database;
using Database.Models;
using System.Collections.Generic;
using System.Linq;
using VideoHandler.Attributes;
using VideoHandler.Models;

namespace VideoHandler.SearchFilters
{
    [Filter(FilterType.RatingOrSafer)]
    public class RatingOrSaferFilter : IFilter
    {
        private IVideoRepository _repo;
        public RatingOrSaferFilter(IVideoRepository repo)
        {
            _repo = repo;
        }

        public List<Video> Filter(SearchParameter parameter)
        {
            return Rating(parameter.Value);
        }

        public List<Video> Rating(string rating)
        {
            rating = rating.ToLower();
            var selectedRating = _repo.GetRatings().FirstOrDefault(r => r.Name.ToLower() == rating);

            List<Video> result = _repo.GetViewableVideos().Where(x => x.RatingId <= selectedRating.Id).ToList();

            return result.ToList();

        }
    }
}
