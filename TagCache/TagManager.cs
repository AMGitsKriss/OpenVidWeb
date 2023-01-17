using System.Collections.Generic;

namespace TagCache
{
    public class TagManager
    {
        private SuggestedTags _suggested;
        private RelatedTags _related;

        public TagManager(SuggestedTags suggested, RelatedTags related)
        {
            _suggested = suggested;
            _related = related;
        }

        public List<string> GetRelatedTags(string tag)
        {
            return _related.Get(tag);
        }

        public List<string> GetTagsInName(string videoName)
        {
            return _related.GetTagsInName(videoName);
        }

        public List<string> GetAllUsedTags()
        {
            return _suggested.Get();
        }
    }
}
