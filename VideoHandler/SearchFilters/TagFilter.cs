using Database;
using Database.Models;
using System.Collections.Generic;
using System.Linq;
using VideoHandler.Attributes;
using VideoHandler.Models;

namespace VideoHandler.SearchFilters
{
    [Filter(FilterType.Tag)]
    public class TagFilter : IFilter
    {
        private IVideoRepository _repo;
        public TagFilter(IVideoRepository repo)
        {
            _repo = repo;
        }

        public List<Video> Filter(SearchParameter parameter)
        {
            return Tag(parameter.Value, parameter.InvertSearch);
        }

        public List<Video> Tag(string tag, bool invert)
        {
            var tagObject = _repo.GetAllTags().FirstOrDefault(x => x.Name.ToLower() == tag.ToLower());

            List<Video> result;
            if(tagObject == null)
            {
                result = new List<Video>();
            }
            else if (invert)
            {
                //var ids = result.Select(x => x.Id);
                result = _repo.GetViewableVideos().Where(x => !x.VideoTag.Select(t => t.TagId).Contains(tagObject.Id)).ToList();
            }
            else
            {
                result = _repo.TagsWithVideos().Where(x => x.TagId == tagObject.Id && !x.Video.IsDeleted).Select(x => x.Video).ToList();
            }
            return result.ToList();

        }
    }
}
