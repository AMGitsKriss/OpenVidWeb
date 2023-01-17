using Microsoft.AspNetCore.Mvc;
using OpenVid.Areas.Playback.Models.Search;
using OpenVid.Extensions;
using OpenVid.Models.Shared;
using OrionDashboard.Web.Attributes;
using System.Linq;
using VideoHandler;

namespace OpenVid.Areas.Playback.Controllers
{
    [RequireLogin]
    [Area("playback")]
    public class SearchController : OpenVidController
    {
        private IVideoManager _videoManager;
        private ISearchManager _search;

        public SearchController(IVideoManager videoManager, ISearchManager search)
        {
            _videoManager = videoManager;
            _search = search;
        }

        [Route("[controller]/{searchString}")]
        public IActionResult Index(string searchString)
        {
            SearchViewModel viewModel = new SearchViewModel();

            viewModel.SearchString = searchString;

            // TODO - Tag sidebar should be an ajax query. Then we can remove this.
            var videoIDs = _search.PaginatedQuery(searchString, 1, out var totalPages).Select(v => v.Id).ToList();

            var selectedVideos = _videoManager.GetVideos().Where(x => videoIDs.Contains(x.Id));
            var tags = selectedVideos.SelectMany(x => x.VideoTag);
            var tagIDs = tags.Select(x => x.TagId).ToList();

            var allTags = _videoManager.GetAllTags().Where(x => tagIDs.Contains(x.Id)).GroupBy(t => t.Type).OrderBy(t => (t.Key ?? 0));
            viewModel.TagGroups = allTags.Select(t => new TagViewModel()
            {
                Category = t.FirstOrDefault()?.TypeNavigation?.Name ?? "Tags",
                Tags = t.ToList()
            }).ToList();

            return View(viewModel);
        }
    }
}
