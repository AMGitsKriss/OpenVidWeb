using CatalogManager.Entities;
using CatalogManager.Helpers;
using CatalogManager.Metadata;
using CatalogManager.Models;
using Database;
using Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogManager
{
    public class ImportService
    {
        private readonly CatalogImportOptions _configuration;
        private readonly IVideoRepository _repository;
        private readonly IMetadataStrategy _metadata;

        public ImportService(IOptions<CatalogImportOptions> configuration, IVideoRepository repository, IMetadataStrategy metadata)
        {
            _configuration = configuration.Value;
            _repository = repository;
            _metadata = metadata;
        }

        public List<FoundVideo> FindFiles()
        {
            var importDir = Path.Combine(_configuration.ImportDirectory, "01_ingest");
            Directory.CreateDirectory(importDir);
            return FindFiles(importDir, importDir);
        }

        public List<FoundVideo> GetQueuedFiles()
        {
            var queue = _repository.GetPendingEncodeQueue();

            return queue.Select(q => new FoundVideo()
            {
                FileName = Path.GetFileNameWithoutExtension(q.InputDirectory),
                Resolution = $"{q.MaxHeight}p",
                FullName = q.InputDirectory,
                PlaybackFormat = q.PlaybackFormat
            }).ToList();
        }

        public List<SegmentJob> GetPendingSegmenting()
        {
            // Return a list of summaries for pending segment jobs. 
            return _repository.GetPendingSegmentJobs().Select(v => new SegmentJob()
            {
                JobId = v.Id,
                VideoId = v.VideoId,
                Name = v.Video.Name,
                IsReady = v.IsReady
            }).ToList();
        }

        public List<FoundVideo> FindFiles(string dir, string prefix)
        {
            try
            {
                var result = new List<FoundVideo>();

                var suggestedTags = dir.Replace(prefix, "").Split( new[] {@"\", " "}, StringSplitOptions.RemoveEmptyEntries).ToList();

                // Files
                foreach (var file in Directory.GetFiles(dir))
                {
                    var info = new FileInfo(file);
                    var video = new FoundVideo()
                    {
                        FileName = info.Name,
                        FullName = info.FullName,
                        FileLocation = info.DirectoryName,
                        SuggestedTags = suggestedTags,
                        Size = info.Length
                    };
                    result.Add(video);
                }

                // Directories
                foreach (var folder in Directory.GetDirectories(dir))
                {
                    result.AddRange(FindFiles(folder, prefix));
                }

                return result;
            }
            catch (DirectoryNotFoundException)
            {

                return FindFiles(dir, prefix);
            }
        }

        public async Task<bool> UploadFile(IFormFile file)
        {
            string targetFolder = Path.Combine(_configuration.ImportDirectory, "01_ingest");

            Helpers.FileHelpers.TouchDirectory(targetFolder);

            string targetPath = Path.Combine(targetFolder, file.FileName);
            using Stream fileStream = new FileStream(targetPath, FileMode.Create);

            try
            {
                await file.CopyToAsync(fileStream);
                return true;
            }
            catch (Exception ex)
            {
                File.Delete(targetPath);
                return false;
            }
        }

        public void IngestFiles()
        {
            var pendingFiles = FindFiles();

            var queuedDirectory = Path.Combine(_configuration.ImportDirectory, "02_queued");

            foreach (var pending in pendingFiles)
            {
                // DATABASE
                var videoId = CreateVideoInDatabase(pending);
                if (videoId == 0)
                    continue;

                var newFileName = EncodeJobContext.SaveSafeFileName(pending.FileName);
                if (!MoveFileToDirectory(pending.FullName, queuedDirectory, newFileName))
                    _repository.DeleteVideo(videoId);
            }
        }

        private int CreateVideoInDatabase(FoundVideo pending)
        {
            var tags = _repository.DefineTags(pending.SuggestedTags);

            var meta = _metadata.GetMetadata(pending.FullName);
            var toSave = new Video()
            {
                Name = Path.GetFileNameWithoutExtension(pending.FileName),
                Length = meta.Duration,
                VideoEncodeQueue = new List<VideoEncodeQueue>(),
                VideoSegmentQueue = new List<VideoSegmentQueue>(),
                VideoTag = tags.Select(t => new VideoTag()
                {
                    Tag = t
                }).ToList()
            };

            toSave = _repository.SaveVideo(toSave);

            // If our source is 720p, don't bother trying to use the 1080p preset.
            var presets = GetPresets(pending.FullName);

            foreach (var preset in presets)
            {
                var newFileName = EncodeJobContext.SaveSafeFileName(pending.FileName);
                var newFileNameWithoutExtension = Path.GetFileNameWithoutExtension(newFileName);
                toSave.VideoEncodeQueue.Add(new VideoEncodeQueue()
                {
                    VideoId = toSave.Id,
                    InputDirectory = newFileName,
                    OutputDirectory = $"{newFileNameWithoutExtension}_{preset.MaxHeight}.mp4",
                    Encoder = preset.Encoder,
                    RenderSpeed = preset.RenderSpeed,
                    VideoFormat = preset.VideoFormat,
                    PlaybackFormat = preset.PlaybackFormat,
                    Quality = preset.Quality,
                    MaxHeight = preset.MaxHeight,
                    IsVertical = meta.Height > meta.Width
                });
            }

            if (presets.Any(p => p.PlaybackFormat == "dash"))
            {
                var segmentJob = new VideoSegmentQueue()
                {
                    VideoId = toSave.Id,
                    VideoSegmentQueueItem = new List<VideoSegmentQueueItem>()
                };
                toSave.VideoSegmentQueue.Add(segmentJob);

                var subtitleSaveDir = Path.Combine(_configuration.ImportDirectory, "04_shaka_packager", Path.GetFileNameWithoutExtension(EncodeJobContext.SaveSafeFileName(pending.FileName)));
                Helpers.FileHelpers.TouchDirectory(subtitleSaveDir);

                var subtitleBackupDir = Path.Combine(_configuration.BucketDirectory, "Subtitles", toSave.Id.ToString().PadLeft(2, '0').Substring(0, 2), toSave.Id.ToString());

                // TODO - This is file system work. Should not be in database function. Can it be moved to the Encoder App?
                var subtitleFiles = _metadata.FindSubtitles(pending.FullName).ToList();
                
                foreach (var subtitle in subtitleFiles)
                {
                    Helpers.FileHelpers.TouchDirectory(subtitleBackupDir);
                    _metadata.ExtractSubtitles(subtitle, subtitleSaveDir);
                    _metadata.ExtractSubtitles(subtitle, subtitleBackupDir, false);

                    segmentJob.VideoSegmentQueueItem.Add(new VideoSegmentQueueItem()
                    {
                        VideoId = toSave.Id,
                        ArgStream = "text",
                        ArgInputFile = $"{subtitle.OutputFileName}.vtt",
                        ArgInputFolder = Path.Combine("04_shaka_packager", Path.GetFileNameWithoutExtension(EncodeJobContext.SaveSafeFileName(pending.FileName))),
                        ArgStreamFolder = $"subtitle_{subtitle.Language}"
                    });
                }
            }

            toSave = _repository.SaveVideo(toSave);

            return toSave.Id;
        }

        private List<EncoderPresetOptions> GetPresets(string fileFullName)
        {
            // TODO - This is gnarly. Is there a nicer way to do it?
            // TODO - Base this on width instead of height. Movies seem to be 1920x800, not *really* 1080p
            var results = new List<EncoderPresetOptions>();
            var metadata = _metadata.GetMetadata(fileFullName);

            // 1080p ultrawide is usually more like 800p with a 1920 width. This way we'll pretend it has black bars so we get the width consistent
            int heightIfSizteenNine = (int)Math.Ceiling(metadata.Width * 0.5625d);

            // Find MP4 Presets
            var mp4Presets = _configuration.EncoderPresets.Where(v => v.MaxHeight <= heightIfSizteenNine && v.PlaybackFormat == "mp4").ToList();
            var smallestmp4Preset = _configuration.EncoderPresets.Where(v => v.PlaybackFormat == "mp4").OrderBy(v => v.MaxHeight).FirstOrDefault();

            // Find MPD PResets
            var mpdPresets = _configuration.EncoderPresets.Where(v => v.MaxHeight <= heightIfSizteenNine && v.PlaybackFormat == "dash").ToList();
            var smallestmpdPreset = _configuration.EncoderPresets.Where(v => v.PlaybackFormat == "dash").OrderBy(v => v.MaxHeight).FirstOrDefault();

            // Set MP4
            if (!mp4Presets.Any() && smallestmp4Preset != null)
            {
                if (mpdPresets.Any() && smallestmp4Preset.MaxHeight > mpdPresets.Select(m => m.MaxHeight).Max())
                    smallestmp4Preset.MaxHeight = mpdPresets.Select(m => m.MaxHeight).Max();
                else if (smallestmpdPreset != null && smallestmp4Preset.MaxHeight > smallestmpdPreset.MaxHeight)
                    smallestmp4Preset.MaxHeight = smallestmpdPreset.MaxHeight;
                results.Add(smallestmp4Preset);
            }
            else
            {
                results.AddRange(mp4Presets);
            }

            // Set MPD
            if (!mpdPresets.Any() && smallestmpdPreset != null)
                results.Add(smallestmpdPreset);
            else
                results.AddRange(mpdPresets);

            return results;
        }

        private bool MoveFileToDirectory(string sourceFullName, string targetDirectory, string destinationFileName)
        {
            try
            {
                if (File.Exists(sourceFullName))
                {
                    if (!Directory.Exists(targetDirectory))
                        Directory.CreateDirectory(targetDirectory);
                    File.Move(sourceFullName, Path.Combine(targetDirectory, destinationFileName));
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public void SaveSubtitle(int videoId, string language, string fileName, byte[] file)
        {
            var video = _repository.GetVideo(videoId);

            var extention = Path.GetExtension(fileName);
            var newFileName = EncodeJobContext.SaveSafeFileName(fileName);

            var packagerFolderName = Path.GetFileNameWithoutExtension(video.VideoEncodeQueue.First().InputDirectory);
            var subtitleSaveDir = Path.Combine(_configuration.ImportDirectory, "04_shaka_packager", packagerFolderName);
            Helpers.FileHelpers.TouchDirectory(subtitleSaveDir);

            var subtitleBackupDir = Path.Combine(_configuration.BucketDirectory, "subtitles", videoId.ToString().PadLeft(2, '0').Substring(0, 2), videoId.ToString());
            Helpers.FileHelpers.TouchDirectory(Path.GetDirectoryName(subtitleBackupDir));
            File.WriteAllText("myFile.txt", System.Text.Encoding.UTF8.GetString(file));

            if (extention == ".vtt")
            {
                // TODO - Save as is
            }
            if (extention == ".srt")
            {
                newFileName = newFileName.Replace(".srt", ".vtt");
                SrtHelper.ConvertSrtToVtt(file, Path.Combine(subtitleSaveDir, newFileName));
            }

            video.VideoSegmentQueueItem.Add(new VideoSegmentQueueItem()
            {
                VideoId = video.Id,
                VideoSegmentQueueId = video.VideoSegmentQueue.Last().Id,
                ArgStream = "text",
                ArgInputFile = newFileName,
                ArgInputFolder = Path.Combine("04_shaka_packager", packagerFolderName),
                ArgStreamFolder = $"subtitle_{language}"
            });

            _repository.SaveVideo(video);
        }

    }
}