using Database.Models;
using System.Collections.Generic;
using VideoHandler.Models;

namespace VideoHandler.SearchFilters
{
    public interface IFilter
    {
        List<Video> Filter(SearchParameter parameter);
    }
}
