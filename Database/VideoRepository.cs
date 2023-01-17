using Database.Extensions;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Database
{
    // Add-Migration MIGRATION_NAME -verbose -Context OpenVidContext
    // Scaffold-DbContext "name=ConnectionStrings:DefaultDatabase" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force
    public class VideoRepository : IVideoRepository
    {
        OpenVidContext _context;

        public VideoRepository(OpenVidContext context)
        {
            _context = context;
        }

        public IQueryable<Video> GetAllVideos()
        {
            return _context.Video.Include(v => v.VideoSource);
        }

        public IQueryable<Video> GetViewableVideos()
        {
            return GetAllVideos().Include(x => x.VideoTag).ThenInclude(x => x.Video).ThenInclude(x => x.Rating).Where(v => !v.IsDeleted && v.VideoSource.Any()).OrderByDescending(x => x.Id);
        }

        public IQueryable<Video> GetSoftDeletedVideos()
        {
            return GetAllVideos().Where(v => v.IsDeleted).OrderByDescending(x => x.Id);
        }

        public Video GetVideo(string md5)
        {
            return GetAllVideos().Include(x => x.VideoTag).ThenInclude(x => x.Tag).FirstOrDefault(x => x.VideoSource.Any(v => v.Md5 == md5));
        }

        public VideoSource GetVideoSource(string md5)
        {
            return _context.VideoSource.Include(v => v.Video).FirstOrDefault(x => x.Md5 == md5);
        }

        public Video GetVideo(int id)
        {
            return GetAllVideos().Include(x => x.VideoSource).Include(x => x.VideoTag).ThenInclude(x => x.Tag).Include(x => x.VideoEncodeQueue).Include(x => x.VideoSegmentQueue).Include(x => x.VideoSegmentQueueItem).FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<Tag> GetAllTags()
        {
            var result = _context.Tag.Include(x => x.TypeNavigation).Include(x => x.VideoTag).ThenInclude(x => x.Video).ThenInclude(x => x.VideoSource).Where(x => x.VideoTag.Any(v => !v.Video.IsDeleted && v.Video.VideoSource.Any())).OrderByDescending(x => x.VideoTag.Count()).ThenBy(x => x.Name);
            return result;
        }

        public List<Ratings> GetRatings()
        {
            return _context.Ratings.ToList();
        }

        public IQueryable<VideoTag> TagsWithVideos()
        {
            var result = _context.VideoTag.Include(x => x.Video);
            return result;
        }

        public void RemoveTagsFromVideo(IEnumerable<VideoTag> tagsToRemove)
        {
            _context.VideoTag.RemoveRange(tagsToRemove);
            _context.SaveChanges();
        }

        public void AddTagsToVideo(IEnumerable<VideoTag> tagsToAdd)
        {
            _context.VideoTag.AddRange(tagsToAdd);
            _context.SaveChanges();
        }

        public IQueryable<Video> Search(string search)
        {
            var query = search.Trim().ToLower().Split(" ");
            var result = (from t in _context.Tag
                          join vt in _context.VideoTag on t.Id equals vt.TagId
                          join v in _context.Video on vt.VideoId equals v.Id
                          where query.Contains(t.Name.ToLower()) || query.Contains(v.Name)
                          group v by v.Id into g
                          from vg in g
                          select vg);
            return result;
        }

        public Video SaveVideo(Video video)
        {
            try
            {
                if (video.Id == 0)
                {
                    _context.Video.Add(video);
                }
                else
                {
                    _context.Video.Update(video);
                }

                var updateCount = _context.SaveChanges();

                return video;
            }
            catch (Exception ex)
            {
                var test = ex.Message;
                throw ex;
            }
        }


        public VideoSource SaveVideoSource(VideoSource videoSource)
        {
            try
            {
                if (videoSource.Id == 0)
                {
                    _context.VideoSource.Add(videoSource);
                }
                else
                {
                    _context.VideoSource.Update(videoSource);
                }

                _context.SaveChanges();

                return videoSource;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SoftDelete(int id)
        {
            Video video = GetVideo(id);

            video.IsDeleted = !video.IsDeleted;

            SaveVideo(video);

            return true;
        }

        public bool DeleteSource(string md5)
        {
            VideoSource video = GetVideoSource(md5);

            _context.VideoSource.Remove(video);

            _context.SaveChanges();

            return true;
        }

        public virtual bool DeleteVideo(int id)
        {
            Video video = GetVideo(id);

            _context.Video.Remove(video);

            _context.SaveChanges();

            return true;
        }

        public IQueryable<Tag> DefineTags(List<string> tags)
        {
            try
            {
                var existingTags = _context.Tag.Select(x => x.Name.ToLower());

                foreach (var unsafeTag in tags)
                {
                    string tag = unsafeTag.Trim().ToLower();
                    if (!existingTags.Contains(tag))
                    {
                        _context.Tag.Add(new Tag() { Name = tag });
                    }
                }

                _context.SaveChanges();

                var tagsInDatabase = _context.Tag.Where(x => tags.Contains(x.Name));
                return tagsInDatabase;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool SaveEncodeJob(VideoEncodeQueue encodeJob)
        {
            if (encodeJob.Id == 0)
            {
                _context.Add(encodeJob);
            }
            else
            {
                _context.Update(encodeJob);
            }

            _context.SaveChanges();

            return encodeJob.Id != 0;
        }

        public bool SaveSegmentJob(VideoSegmentQueue segmentJob)
        {
            if (segmentJob.Id == 0)
            {
                _context.Add(segmentJob);
            }
            else
            {
                _context.Update(segmentJob);
            }

            _context.SaveChanges();

            return segmentJob.Id != 0;
        }

        public bool SaveSegmentItem(VideoSegmentQueueItem segmentJob)
        {
            if (segmentJob.Id == 0)
            {
                _context.Add(segmentJob);
            }
            else
            {
                _context.Update(segmentJob);
            }

            _context.SaveChanges();

            return segmentJob.Id != 0;
        }

        public IQueryable<VideoEncodeQueue> GetPendingEncodeQueue()
        {
            return _context.VideoEncodeQueue.Where(v => !v.IsDone);
        }

        public IQueryable<VideoEncodeQueue> GetAllEncodeQueue()
        {
            return _context.VideoEncodeQueue;
        }

        public bool IsFileStillNeeded(int videoId)
        {
            return _context.VideoEncodeQueue.Any(v =>
                v.VideoId == videoId &&
                !v.IsDone);
        }

        public IQueryable<VideoEncodeQueue> GetIncompleteDashJobs(int videoId)
        {
            var isAnyIncomplete = _context.VideoEncodeQueue.Where(v =>
                v.VideoId == videoId &&
                v.PlaybackFormat == "dash" &&
                !v.IsDone);

            return isAnyIncomplete;
        }

        public IEnumerable<IEnumerable<VideoSegmentQueueItem>> GetPendingSegmentItems()
        {
            // Return segments for videos that are done, but also for videos that are finished encoding. 
            var pendingEncodes = _context.VideoEncodeQueue.Where(e => !e.IsDone).Select(e => e.VideoId).Distinct();

            var result = _context.VideoSegmentQueueItem.Where(s => !pendingEncodes.Contains(s.VideoId) && !s.VideoSegmentQueue.IsDone);

            var firstBatch = result.ToList().GroupBy(r => r.VideoId).Select(g => g.Select(x => x));

            return firstBatch;
        }

        public IQueryable<VideoSegmentQueue> GetPendingSegmentJobs()
        {
            return _context.VideoSegmentQueue.Include(j => j.Video).Where(j => !j.IsDone);
        }

        public IQueryable<VideoSegmentQueue> GetSegmentJobsForVideo(int videoId)
        {
            return _context.VideoSegmentQueue.Where(q => q.VideoId == videoId);
        }

        public TagType GetCategory(string categoryName)
        {
            return _context.TagType.FirstOrDefault(c => c.Name.ToLower() == categoryName.ToLower());
        }
    }
}
