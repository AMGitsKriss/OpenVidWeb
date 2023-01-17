using CacheMeIfYouCan;
using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TagCache
{
    public class SuggestedTags
    {
        private readonly IVideoRepository _repository;
        private static ICachedObject<List<string>> _allUsedTags;

        public SuggestedTags(IVideoRepository repository)
        {
            _repository = repository;
        }

        public List<string> Build()
        {
            List<string> validTags = _repository.GetAllTags().Where(t => t.VideoTag.Any()).Select(t => t.Name).ToList();

            return validTags;
        }

        public List<string> Get()
        {
            
            if (_allUsedTags == null)
                Initialize();

            return _allUsedTags.Value ?? new List<string>();
        }

        public void Refresh()
        {
            _allUsedTags.RefreshValue(TimeSpan.MinValue);
        }

        public async Task Initialize()
        {

            _allUsedTags = CachedObjectFactory
                                        .ConfigureFor(Build)
                                        .WithRefreshInterval(TimeSpan.FromHours(1))
                                        .Build();

            await _allUsedTags.InitializeAsync();
        }
    }
}
