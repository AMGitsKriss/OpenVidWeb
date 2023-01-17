using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OpenVid.Areas.Playback.Models.VideoList;
using OpenVid.Models.Shared;
using OrionDashboard.Web.Attributes;
using System.Linq;
using VideoHandler;

namespace OpenVid.Areas.Playback.Controllers
{
    [RequireLogin]
    [Area("Playback")]
    public class VideoListController : Controller
    {
        private readonly ISearchManager _search;
        private readonly IConfiguration _configuration;

        public VideoListController(ISearchManager search, IConfiguration configuration)
        {
            _search = search;
            _configuration = configuration;
        }

        public IActionResult Index(int pageNo = 0, string searchString = "")
        {
            VideoListViewModel viewModel = new VideoListViewModel()
            {
                Videos = _search.PaginatedQuery(searchString ?? "", pageNo, out var totalPages).Select(v => new VideoViewModel()
                {
                    Id = v.Id,
                    Name = v.Name,
                    Length = v.Length.Hours > 0 ? string.Format("{0:00}:{1:00}:{2:00}", v.Length.Hours, v.Length.Minutes, v.Length.Seconds) : string.Format("{0:00}:{1:00}", v.Length.Minutes, v.Length.Seconds)
                }).ToList(),
                TotalPages = totalPages,
                HasNextPage = pageNo < totalPages,
                CurrentPage = pageNo,
                SearchQuery = searchString
            };
            //return PartialView("VideoListEndless", viewModel);
            return PartialView("_VideoListPaginated", viewModel);
        }
    }
}
