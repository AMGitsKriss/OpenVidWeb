using Database;
using Database.Models;
using System.Collections.Generic;
using System.Linq;
using VideoHandler.Attributes;
using VideoHandler.Models;

namespace VideoHandler.SearchFilters
{
    [Filter(FilterType.Rating)]
    public class RatingFilter : IFilter
    {
        private IVideoRepository _repo;
        public RatingFilter(IVideoRepository repo)
        {
            _repo = repo;
        }

        public List<Video> Filter(SearchParameter parameter)
        {
            return Rating(parameter.Value, parameter.InvertSearch);
        }

        public List<Video> Rating(string rating, bool invert)
        {
            rating = rating.ToLower();
            var isNullRating = (rating == "none" || rating == "unrated" || rating == "null");

            List<Video> result;
            if (isNullRating && !invert)
            {
                result = _repo.GetViewableVideos().Where(x => x.RatingId == null).ToList();
            }
            else if (isNullRating && invert)
            {
                result = _repo.GetViewableVideos().Where(x => x.RatingId != null).ToList();
            }
            else if (invert)
            {
                //var ids = result.Select(x => x.Id);
                result = _repo.GetViewableVideos().Where(x => !(x.Rating.Name.ToLower() == rating)).ToList();
            }
            else
            {
                result = _repo.GetViewableVideos().Where(x => x.Rating.Name.ToLower() == rating).ToList();
            }
            return result.ToList();

        }
    }
}
