using Database.Models;
using System.IO;
using System.Text.RegularExpressions;

namespace CatalogManager.Entities
{
    public class EncodeJobContext
    {
        private readonly CatalogImportOptions _configuration;

        private readonly string _ingest = "01_ingest";
        private readonly string _queued = "02_queued";
        private readonly string _transcoded = "03_transcode_complete";
        private readonly string _packager = "04_shaka_packager";

        public EncodeJobContext(CatalogImportOptions configuration, VideoEncodeQueue videoEncodeQueue)
        {
            _configuration = configuration;
            QueueItem = videoEncodeQueue;
        }

        public VideoEncodeQueue QueueItem { get; set; }

        public static string SaveSafeFileName(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            fileName = Path.GetFileNameWithoutExtension(fileName);
            fileName = Regex.Replace(fileName, @" ", "_");
            fileName = Regex.Replace(fileName, @"[,~!?\-]|\[(.*?)\]|\((.*?)\)", string.Empty);
            fileName = Regex.Replace(fileName, @"_+", "_");
            fileName = fileName.Trim('_');
            return $"{fileName}{extension}";
        }

        // INGEST
        public string FolderIngest
        {
            get
            {
                return Path.Combine(_configuration.ImportDirectory, _ingest);
            }
        }
        public string FileIngest
        {
            get
            {
                return Path.Combine(FolderIngest, QueueItem.InputDirectory);
            }
        }

        // QUEUED
        public string FolderQueued
        {
            get
            {
                return Path.Combine(_configuration.ImportDirectory, _queued);
            }
        }
        public string FileQueued
        {
            get
            {
                return Path.Combine(FolderQueued, QueueItem.InputDirectory);
            }
        }

        // TRANSCODED
        public string FolderTranscoded
        {
            get
            {
                return Path.Combine(_configuration.ImportDirectory, _transcoded);
            }
        }
        public string FileTranscoded
        {
            get
            {
                return Path.Combine(FolderTranscoded, QueueItem.OutputDirectory);
            }
        }

        // PENDING PACKAGING
        public string FolderPackager
        {
            get
            {
                return Path.Combine(_configuration.ImportDirectory, FolderRelativePackager);
            }
        }
        public string FolderRelativePackager
        {
            get
            {
                return Path.Combine(_packager, InputFileName);
            }
        }
        public string FilePackager
        {
            get
            {
                return Path.Combine(FolderPackager, QueueItem.OutputDirectory);
            }
        }

        public string InputFileName => Path.GetFileNameWithoutExtension(QueueItem.InputDirectory);
        public string InputExtension => Path.GetExtension(QueueItem.InputDirectory);

        public string OutputFileName => Path.GetFileNameWithoutExtension(QueueItem.OutputDirectory);
        public string OutputExtension => Path.GetExtension(QueueItem.OutputDirectory);
    }
}
