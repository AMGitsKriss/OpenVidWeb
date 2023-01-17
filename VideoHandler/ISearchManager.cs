using Database.Models;
using System.Collections.Generic;
using VideoHandler.Models;

namespace VideoHandler
{
    public interface ISearchManager
    {
        List<SearchParameter> MapSearchQueryToParameters(string searchQuery);
        List<Video> PaginatedQuery(string searchQuery, int pageNumber, out int totalPages);
        List<Video> Query(string searchQuery);
    }
}