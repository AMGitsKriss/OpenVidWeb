using Database;
using Database.Models;
using System.Collections.Generic;
using System.Linq;
using VideoHandler.Attributes;
using VideoHandler.Models;

namespace VideoHandler.SearchFilters
{
    [Filter(FilterType.Category)]
    public class CategoryFilter : IFilter
    {
        private IVideoRepository _repo;
        public CategoryFilter(IVideoRepository repo)
        {
            _repo = repo;
        }

        public List<Video> Filter(SearchParameter parameter)
        {
            return Category(parameter.Value, parameter.InvertSearch);
        }

        public List<Video> Category(string categoryName, bool invert)
        {
            TagType category = _repo.GetCategory(categoryName);

            List<Video> result;
            if(category == null)
            {
                result = new List<Video>();
            }
            else if (invert)
            {
                result = _repo.GetViewableVideos().Where(x => !x.VideoTag.Select(t => t.Tag.Type).Contains(category.Id)).ToList();
            }
            else
            {
                result = _repo.GetViewableVideos().Where(x => x.VideoTag.Select(t => t.Tag.Type).Contains(category.Id)).ToList();
            }
            return result.ToList();

        }
    }
}
