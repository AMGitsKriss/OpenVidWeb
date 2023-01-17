using CacheMeIfYouCan;
using Database;
using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TagCache
{
    public class RelatedTags
    {
        private readonly IVideoRepository _repository;
        private static ICachedObject<Dictionary<string, List<string>>> _relatedTags;

        public RelatedTags(IVideoRepository repository)
        {
            _repository = repository;
        }

        public async Task<Dictionary<string, List<string>>> Build()
        {
            var tags = _repository.GetAllTags().ToList();
            Dictionary<string, List<string>> relatedTags = new Dictionary<string, List<string>>();

            foreach (var tag in tags)
            {
                var tagGroup = GetMutualTags(tag).GroupBy(t => t).OrderByDescending(t => t.Count()).Select(t => t.Key).Take(15).ToList();

                relatedTags.TryAdd(tag.Name, tagGroup);
            }

            return relatedTags;
        }

        private IQueryable<string> GetMutualTags(Tag tag)
        {
            var videoTags = _repository.TagsWithVideos().Where(t => t.TagId == tag.Id);
            var allTags = videoTags.SelectMany(vt => vt.Video.VideoTag).Where(t => t.TagId != tag.Id && t.Tag.Type == 0).Select(t => t.Tag.Name);

            return allTags;
        }

        public List<string> Get(string tag)
        {
            if (_relatedTags == null)
                Initialize();

            _relatedTags.Value.TryGetValue(tag, out var result);
            return result ?? new List<string>();
        }

        public List<string> GetTagsInName(string videoName)
        {
            var name = videoName.ToLower().Replace(" ", "_");

            var tagsInTitle = _repository.GetAllTags()
                .Where(t => name.Contains(t.Name))
                .Select(t => t.Name)
                .ToList();

            return tagsInTitle;
        }

        public void Refresh()
        {
            _relatedTags.RefreshValue(TimeSpan.MinValue);
        }

        public async Task Initialize()
        {

            _relatedTags = CachedObjectFactory
                                        .ConfigureFor(Build)
                                        .WithRefreshInterval(TimeSpan.FromHours(1))
                                        .Build();

            await _relatedTags.InitializeAsync();
        }
    }
}
