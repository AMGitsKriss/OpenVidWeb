using CatalogManager.Metadata;
using Database;
using Database.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using VideoHandler.Models;

namespace VideoHandler
{
    public class VideoManager : IVideoManager
    {
        private readonly IConfiguration _configuration;
        private readonly IVideoRepository _repository;
        private readonly IMetadataStrategy _ffmpeg;

        public VideoManager(IVideoRepository repository, IConfiguration configuration, IMetadataStrategy ffmpeg)
        {
            _configuration = configuration;
            _repository = repository;
            _ffmpeg = ffmpeg;
        }

        public IEnumerable<Video> GetVideos()
        {
            return _repository.GetViewableVideos();
        }

        public IEnumerable<Tag> DefineTags(List<string> suggestedTags)
        {
            return _repository.DefineTags(suggestedTags);
        }

        public IEnumerable<Tag> SaveTagsForVideo(Video video, IEnumerable<Tag> tags)
        {
            try
            {
                var tagIDs = tags.Select(x => x.Id);
                var removeTags = _repository.TagsWithVideos().Where(x => video.Id == x.VideoId && !tagIDs.Contains(x.TagId)).ToList();
                //var removeTags = _context.VideoTag.Where(x => video.Id == x.VideoId && !tagIDs.Contains(x.TagId)).ToList();
                var existingTags = _repository.TagsWithVideos().Where(x => video.Id == x.VideoId && tagIDs.Contains(x.TagId)).Select(x => x.TagId).ToList();

                _repository.RemoveTagsFromVideo(removeTags);

                var tagsToAdd = tags.Where(t => !existingTags.Contains(t.Id)).Select(t => new VideoTag() { TagId = t.Id, VideoId = video.Id });
                _repository.AddTagsToVideo(tagsToAdd);

                return tags;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Ratings> GetRatings()
        {
            return _repository.GetRatings();
        }

        public Video GetVideo(int id)
        {
            return _repository.GetVideo(id);
        }

        public async Task<SaveVideoResponse> ImportVideoAsync(ImportVideoRequest request)
        {
            string md5 = GenerateHash(request.FileNameFull);
            string subFolder = md5.Substring(0, 2);
            string ext = Path.GetExtension(request.FileName).Replace(".", "");
            Video video = _repository.GetVideo(md5);

            string videoDirectory = $"{_configuration["Urls:BucketDirectory"]}\\video\\{subFolder}\\";
            string thumbnailDirectory = $"{_configuration["Urls:BucketDirectory"]}\\thumbnail\\{subFolder}\\";

            if (video != null)
                return new SaveVideoResponse()
                {
                    Video = video,
                    AlreadyExists = true,
                    Message = $"Video with the hash {md5} already exists. Skipping."
                };

            //THUMBNAIL
            //string thumbPath = Path.Combine(thumbnailDirectory, $"{md5}.jpg");
            //_ffmpeg.CreateThumbnail(request.FileNameFull, thumbPath, TimeSpan.FromSeconds(60));

            var metaData = GetMetadata(request.FileNameFull);
            FileInfo fileInfo = new FileInfo(request.FileNameFull);
            var newVideo = new Video()
            {
                Name = Path.GetFileNameWithoutExtension(request.FileName),
                Length = metaData.Duration,
                VideoSource = new List<VideoSource>()
                    {
                        new VideoSource()
                        {
                            Md5 = md5,
                            Extension = ext,
                            Width = metaData.Width,
                            Height = metaData.Height,
                            Size = fileInfo.Length,
                        }
                    }
            };

            // DATABASE
            try
            {
                _repository.SaveVideo(newVideo);
            }
            catch (Exception ex)
            {
                return new SaveVideoResponse()
                {
                    AlreadyExists = false,
                    Message = $"Video named {request.FileName} couldn't be added to the database. {ex.Message}"
                };
            }

            // FILE
            try
            {
                string destinationFileName = $"{videoDirectory}{md5}.{ext}";
                if (File.Exists(request.FileNameFull))
                {
                    if (!Directory.Exists(videoDirectory))
                        Directory.CreateDirectory(videoDirectory);
                    File.Move(request.FileNameFull, destinationFileName);
                }
            }
            catch (Exception ex)
            {
                return new SaveVideoResponse()
                {
                    AlreadyExists = false,
                    Message = $"Video named {request.FileName} Couldn't be copied to the bucket. Skipping."
                };
            }

            return new SaveVideoResponse()
            {
                Video = newVideo
            };
        }

        public bool HardDeleteVideo(int id)
        {
            Video video = _repository.GetVideo(id);

            foreach (var source in video.VideoSource.ToList())
            {
                var bucketDirectory = $"{_configuration["Urls:BucketDirectory"]}\\video\\{source.Md5.Substring(0, 2)}\\";
                var internalDirectory = $"{_configuration["Urls:InternalDirectory"]}\\video\\";
                var fileName = $"{source.Md5}.{source.Extension}";

                if (File.Exists(bucketDirectory + fileName))
                {
                    File.Delete(bucketDirectory + fileName);
                }
                else if (File.Exists(internalDirectory + fileName))
                {
                    File.Delete(internalDirectory + fileName);
                }

                _repository.DeleteSource(source.Md5);
            }

            video = _repository.GetVideo(id);
            if (!video.VideoSource.Any())
            {
                _repository.DeleteVideo(id);
            }

            return true;
        }

        private string GenerateHash(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var ms = File.OpenRead(filename))
                {
                    byte[] hash = md5.ComputeHash(ms);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        private MediaProperties GetMetadata(string location)
        {
            string cmd = $"-v error -select_streams v:0 -show_entries stream=width,height,duration -show_entries format=duration -of csv=s=x:p=0 \"{location}\"";
            Process proc = new Process();
            proc.StartInfo.FileName = @"c:\ffmpeg\ffprobe.exe";
            proc.StartInfo.Arguments = cmd;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.UseShellExecute = false;
            if (!proc.Start())
            {
                Console.WriteLine("Error starting");
            }
            string outputString = proc.StandardOutput.ReadToEnd();
            string[] metaData = outputString.Trim().Split(new char[] { 'x', '\n' });
            // Remove the milliseconds
            MediaProperties properties = new MediaProperties()
            {
                Width = int.Parse(metaData[0]),
                Height = int.Parse(metaData[1]),
                Duration = TimeSpan.FromSeconds(double.Parse(metaData.Length > 3 ? metaData[3].Trim() : metaData[2].Trim()))
            };
            proc.WaitForExit();
            proc.Close();
            return properties;
        }

        public IEnumerable<Video> GetSoftDeletedVideos()
        {
            return _repository.GetSoftDeletedVideos();
        }

        public Video SaveVideo(Video video)
        {
            return _repository.SaveVideo(video);
        }

        public bool SoftDelete(int id)
        {
            return _repository.SoftDelete(id);
        }

        public IEnumerable<Tag> GetAllTags()
        {
            return _repository.GetAllTags();
        }

        public void SetVideoName(int id, string name)
        {
            var video = GetVideo(id);
            if (video != null)
            {
                video.Name = name;
                SaveVideo(video);
            }
        }
    }
}
