namespace Database.Models
{
    public partial class VideoSegmentQueueItem
    {
        public int Id { get; set; }
        public int VideoId { get; set; }
        public int VideoSegmentQueueId { get; set; }
        /// <summary>
        /// eg. C:\video1
        /// </summary>
        public string ArgInputFolder { get; set; }
        /// <summary>
        /// eg. video.mp4, eng.vtt
        /// </summary>
        public string ArgInputFile { get; set; }
        /// <summary>
        /// eg. audio, video, text
        /// </summary>
        public string ArgStream { get; set; }
        /// <summary>
        /// eg. en, eng, jp, jpn
        /// </summary>
        public string? ArgLanguage { get; set; }

        /// <summary>
        /// eg. audio_eng, 720, subtitles_eng
        /// </summary>
        public string ArgStreamFolder { get; set; }
        public string? ArgStreamId { get; set; }

        public virtual VideoSegmentQueue VideoSegmentQueue { get; set; }
        public virtual Video Video { get; set; }
    }
}
