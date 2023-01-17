using Database;
using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using VideoHandler.Attributes;
using VideoHandler.Models;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace VideoHandler.SearchFilters
{
    [Filter(FilterType.Meta)]
    public class MetaFilter : IFilter
    {
        private IVideoRepository _repo;
        private Regex _alphaNumericalOnly = new Regex("[^a-zA-Z0-9 -]");
        public MetaFilter(IVideoRepository repo)
        {
            _repo = repo;
        }

        public List<Video> Filter(SearchParameter parameter)
        {
            Type thisType = GetType();
            MethodInfo theMethod = thisType.GetMethod(parameter.Value, BindingFlags.NonPublic | BindingFlags.IgnoreCase | BindingFlags.Instance);
            return (List<Video>)theMethod.Invoke(this, null);
        }

        private List<Video> Untagged()
        {
            var result = _repo.GetViewableVideos().Where(x => x.VideoTag.Count() == 0).ToList();
            return result;
        }

        private List<Video> Tagged()
        {
            var result = _repo.GetViewableVideos().Where(x => x.VideoTag.Count() != 0).ToList();
            return result;
        }

        private List<Video> Deleted()
        {
            var result = _repo.GetSoftDeletedVideos().ToList();
            return result;
        }

        private List<Video> SameLength()
        {
            var result = _repo.GetViewableVideos().ToList().GroupBy(m => m, new DurationComparer())
                               .Where(a => a.Count() > 1)
                               .SelectMany(a => a.ToList());
            return result.ToList();
        }

        private List<Video> SameName()
        {
            var result = _repo.GetViewableVideos().ToList().GroupBy(m => _alphaNumericalOnly.Replace(m.Name, ""))
                               .Where(a => a.Count() > 1)
                               .SelectMany(a => a.ToList());
            return result.ToList();
        }

        public class DurationComparer : IEqualityComparer<Video>
        {
            public bool Equals(Video x, Video y)
            {
                int secondsVariance = 3;

                return Math.Abs(x.Length.TotalSeconds - y.Length.TotalSeconds) <= secondsVariance;
            }

            public int GetHashCode([DisallowNull] Video obj)
            {
                return 1;
            }
        }
    }
}
