using CatalogManager;
using Database.Users;
using Microsoft.AspNetCore.Mvc;
using OpenVid.Areas.Catalog.Models.Edit;
using OpenVid.Areas.Playback.Models;
using OpenVid.Areas.Playback.Models.Shared;
using OpenVid.Extensions;
using OpenVid.Models;
using OrionDashboard.Web.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagCache;
using VideoHandler;

namespace OpenVid.Areas.Catalog.Controllers
{
    [Area("Catalog")]
    [RequirePermission(Permissions.Catalog_Update)]
    public class EditController : OpenVidController
    {
        private readonly IVideoManager _videoService;
        private readonly IUrlResolver _urlResolver;
        private readonly TagManager _tagManager;
        private readonly ThumbnailService _thumbnailService;

        public EditController(IVideoManager videoService, IUrlResolver urlResolver, TagManager tagManager, ThumbnailService thumbnailService)
        {
            _videoService = videoService;
            _urlResolver = urlResolver;
            _tagManager = tagManager;
            _thumbnailService = thumbnailService;
        }

        public IActionResult Index(int id)
        {
            var video = _videoService.GetVideo(id);

            if (video == null)
                return NotFound();

            EditViewModel viewModel = new EditViewModel()
            {
                Id = id,
                Name = video.Name,
                VideoSources = _urlResolver.GetVideoUrls(video)
            };

            var tagCollection = video.VideoTag.Select(x => x.Tag.Name).OrderBy(t => t);
            var tagSuggestions = new List<SuggestedTagViewModel>();

            foreach (var item in tagCollection)
            {
                tagSuggestions.Add(new SuggestedTagViewModel()
                {
                    TagName = item,
                    RelatedTags = _tagManager.GetRelatedTags(item).Select(t => new RelatedTagViewModel()
                    {
                        TagName = t,
                        AlreadyUsed = tagCollection.Contains(t)
                    }).ToList()
                });
            }
            tagSuggestions.Add(SuggestTagsFromName(video.Name, tagCollection));

            viewModel.Update = new VideoDetailsViewModel()
            {
                Id = video.Id,
                Name = video.Name,
                Description = video.Description,
                Tags = string.Join(" ", tagCollection) + " ",
                IsFlaggedForDeletion = video.IsDeleted,
                RatingId = video.RatingId ?? 0,
                PossibleRatings = _videoService.GetRatings(),
                SuggestedTags = tagSuggestions,
                Metadata = video.VideoSource.Select(s => new MetadataViewModel()
                {
                    Md5 = s.Md5,
                    Extension = s.Extension,
                    Width = s.Width,
                    Height = s.Height,
                    Size = s.Size,
                    DurationInSeconds = (int)video.Length.TotalSeconds
                }).ToList()
            };

            return View(viewModel);
        }

        public IActionResult VideoDetails(int id)
        {
            var video = _videoService.GetVideo(id);

            var tagCollection = video.VideoTag.Select(x => x.Tag.Name).OrderBy(t => t);
            var tagSuggestions = new List<SuggestedTagViewModel>();

            foreach (var item in tagCollection)
            {
                tagSuggestions.Add(new SuggestedTagViewModel()
                {
                    TagName = item,
                    RelatedTags = _tagManager.GetRelatedTags(item).Select(t => new RelatedTagViewModel()
                    {
                        TagName = t,
                        AlreadyUsed = tagCollection.Contains(t)
                    }).ToList()
                });
            }
            tagSuggestions.Add(SuggestTagsFromName(video.Name, tagCollection));

            var viewModel = new VideoDetailsViewModel()
            {
                Id = video.Id,
                Name = video.Name,
                Description = video.Description,
                Tags = string.Join(" ", tagCollection) + " ",
                IsFlaggedForDeletion = video.IsDeleted,
                RatingId = video.RatingId ?? 0,
                PossibleRatings = _videoService.GetRatings(),
                SuggestedTags = tagSuggestions,
                Metadata = video.VideoSource.Select(s => new MetadataViewModel()
                {
                    Md5 = s.Md5,
                    Extension = s.Extension,
                    Width = s.Width,
                    Height = s.Height,
                    Size = s.Size,
                    DurationInSeconds = (int)video.Length.TotalSeconds
                }).ToList()
            };
            return PartialView("_VideoDetails", viewModel);
        }

        [HttpPost]
        public AjaxResponse VideoDetails(EditVideoRequest request)
        {
            var toSave = _videoService.GetVideo(request.Id);
            if (toSave == null)
                return new AjaxResponse() { Message = "Video not found." };

            try
            {
                toSave.Name = request.Name;
                toSave.Description = request.Description;
                toSave.RatingId = request.RatingId == 0 ? null : request.RatingId;
                _videoService.SaveVideo(toSave);

                var tagList = _videoService.DefineTags((request.Tags?.Trim() ?? string.Empty).Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList());
                _videoService.SaveTagsForVideo(toSave, tagList);
            }
            catch (Exception ex)
            {
                return new AjaxResponse() { Message = ex.Message };
            }

            return new AjaxResponse();
        }

        [HttpPost]
        public AjaxResponse Title(int id, [FromBody] string name)
        {
            try
            {
                _videoService.SetVideoName(id, name);
                return new AjaxResponse();
            }
            catch (Exception ex)
            {
                return new AjaxResponse()
                {
                    Message = ex.Message
                };
            }
        }

        public IActionResult VideoThumbnails(int id)
        {
            var video = _videoService.GetVideo(id);

            var tagCollection = video.VideoTag.Select(x => x.Tag.Name).OrderBy(t => t);
            var tagSuggestions = new List<SuggestedTagViewModel>();

            foreach (var item in tagCollection)
            {
                tagSuggestions.Add(new SuggestedTagViewModel()
                {
                    TagName = item,
                    RelatedTags = _tagManager.GetRelatedTags(item).Select(t => new RelatedTagViewModel()
                    {
                        TagName = t,
                        AlreadyUsed = tagCollection.Contains(t)
                    }).ToList()
                });
            }
            tagSuggestions.Add(SuggestTagsFromName(video.Name, tagCollection));

            var viewModel = new VideoThumbnailsViewModel()
            {
                Id = video.Id
            };
            return PartialView("_VideoThumbnail", viewModel);
        }

        [HttpPost]
        public AjaxResponse SaveThumbnail(UploadThumbnailRequest request)
        {
            try
            {
                if (!request.Image.ContentType.Contains("image") || request.Image.Length == 0)
                    throw new Exception("Incorrect file format");

                using (var fileStream = new MemoryStream())
                {
                    request.Image.CopyTo(fileStream);
                    _thumbnailService.SaveThumbnailForVideo(request.Id, fileStream.ToArray());
                }

                return new AjaxResponse();
            }
            catch (Exception ex)
            {
                return new AjaxResponse()
                {
                    Message = ex.Message + " Are you sure this is a jpeg?"
                };
            }
        }

        [HttpPost]
        public AjaxResponse DeleteThumbnail(int id)
        {
            try
            {
                _thumbnailService.DeleteThumbnailForVideo(id);
                return new AjaxResponse();
            }
            catch (Exception ex)
            {
                return new AjaxResponse()
                {
                    Message = ex.Message
                };
            }
        }

        // TODO - Copied from Playcontroller. This should be in a service.
        private SuggestedTagViewModel SuggestTagsFromName(string videoName, IEnumerable<string> tagsOnVideo)
        {
            return new SuggestedTagViewModel()
            {
                TagName = "Video Title",
                RelatedTags = _tagManager.GetTagsInName(videoName).Select(t => new RelatedTagViewModel()
                {
                    TagName = t,
                    AlreadyUsed = tagsOnVideo.Contains(t)
                }).ToList()
            };
        }
    }
}
