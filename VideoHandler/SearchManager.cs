using Database;
using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using VideoHandler.Attributes;
using VideoHandler.Models;
using VideoHandler.SearchFilters;

namespace VideoHandler
{
    // TODO - This should be a project of it's own.
    public class SearchManager : ISearchManager
    {
        internal IVideoRepository _repo;
        private IEnumerable<IFilter> _filters;

        public SearchManager(IVideoRepository repo, IEnumerable<IFilter> filters)
        {
            _repo = repo;
            _filters = filters;
        }

        public List<Video> PaginatedQuery(string searchQuery, int pageNumber, out int totalPages)
        {
            var allVideos = Query(searchQuery);
            var results = allVideos.Skip((pageNumber - 1) * 48);
            totalPages = (int)Math.Ceiling((allVideos.Count() - 48d) / 48d) + 1;

            return results.Take(48).ToList();
        }

        public List<Video> Query(string searchQuery)
        {
            var parameters = MapSearchQueryToParameters(searchQuery);

            var order = parameters.FirstOrDefault(x => x.Type == FilterType.Order);
            parameters.Remove(order);

            var results = new List<Video>();
            if (parameters.Any())
            {
                foreach (var item in parameters)
                {
                    var filter = FilterAttribute.GetFilter(_filters, item.Type); // TODO - Fail gracefully if a filter is missing

                    var paramResult = filter.Filter(item);

                    if (!results.Any())
                        results.AddRange(paramResult);
                    else
                        results = results.Intersect(paramResult, new Comparer()).ToList();
                }
            }
            else
            {
                results.AddRange(_repo.GetViewableVideos());
            }

            if (order?.Value == "random")
                results = results.OrderBy(x => Guid.NewGuid()).ToList();
            else if (order?.Value == "duration")
                results = results.OrderByDescending(x => x.Length).ToList();
            else if (order?.Value == "duration_asc")
                results = results.OrderBy(x => x.Length).ToList();
            else if (order?.Value == "name")
                results = results.OrderBy(x => x.Name).ToList();
            else if (order?.Value == "name_za")
                results = results.OrderByDescending(x => x.Name).ToList();
            else if (order?.Value == "id_asc")
                results = results.OrderBy(x => x.Id).ToList();
            else if (order?.Value == "size")
                results = results.OrderByDescending(x => x.VideoSource.Sum(s => s.Size)).ToList();
            else if (order?.Value == "size_asc")
                results = results.OrderBy(x => x.VideoSource.Sum(s => s.Size)).ToList();            
            else if (order?.Value == "quality")
                results = results.OrderByDescending(x => x.VideoSource.Max(s => s.Width) * x.VideoSource.Max(s => s.Height) * (x.VideoSource.Max(s => s.Size) / 1024) / x.Length.TotalSeconds / 1000000).ToList();
            else if (order?.Value == "quality_desc")
                results = results.OrderBy(x => x.VideoSource.Max(s => s.Width) * x.VideoSource.Max(s => s.Height) * (x.VideoSource.Max(s => s.Size) / 1024) / x.Length.TotalSeconds / 1000000).ToList();
            else
                results = results.OrderByDescending(x => x.Id).ToList();

            return results;
            // ((x.VideoSource.Width * x.VideoSource.Height * (x.VideoSource.Size / 1024)) / x.VideoSource.DurationInSeconds / 1000000
        }

        public List<SearchParameter> MapSearchQueryToParameters(string searchQuery)
        {
            searchQuery = searchQuery.ToLower().Trim();
            string[] splitQuery = searchQuery.Split(new char[] { ' ', '\n' });
            splitQuery = splitQuery.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

            List<SearchParameter> parameters = new List<SearchParameter>();
            foreach (var item in splitQuery)
            {
                SearchParameter param;

                if (item.Contains(":"))
                    param = TagSearch(item);
                else
                    param = StringSearch(item);

                if (param != null)
                    parameters.Add(param);
            }

            return parameters;
        }

        private SearchParameter TagSearch(string searchValue)
        {
            string[] pair = searchValue.Split(':', 2);
            var value = pair[0];
            var invert = pair[0].IndexOf('-') == 0 ? true : false;

            var isSuccess = Enum.TryParse(typeof(FilterType), pair[0].Trim('-'), true, out var type);
            if (pair.Length == 2)
            {
                value = pair[1];
            }

            if (isSuccess)
            {
                return new SearchParameter()
                {
                    Type = (FilterType)type,
                    Value = value,
                    InvertSearch = invert
                };
            }
            return null;
        }

        private SearchParameter StringSearch(string searchValue)
        {
            var invert = searchValue.IndexOf('-') == 0 ? true : false;

            var p = new SearchParameter()
            {
                Type = FilterType.General,
                Value = searchValue.Trim('-'),
                InvertSearch = invert
            };
            return p;
        }
    }
}
