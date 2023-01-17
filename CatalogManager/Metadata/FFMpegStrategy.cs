using CatalogManager.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CatalogManager.Metadata
{
    public class FFMpegStrategy : IMetadataStrategy
    {
        public MediaProperties GetMetadata(string location)
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
            string errorString = proc.StandardError.ReadToEnd();
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

        // TODO - Fix thumbnails. Test Videos:
        // 14680, 14657, 14560, 14232, 13102, 12044, 11959, 14743
        public async Task CreateThumbnail(string videoPath, string thumbPath, TimeSpan timeIntoVideo)
        {
            var cmd = $" -ss {timeIntoVideo} -y -itsoffset -1 -i \"{videoPath}\" -vcodec mjpeg -frames:v 1 -filter:v \"scale=300:168:force_original_aspect_ratio=decrease,pad=300:168:-1:-1:color=black\" \"{thumbPath}\"";

            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Normal,
                FileName = @"c:\ffmpeg\ffmpeg.exe", // TODO - [ffmpeg] Should be configurable.What if I want to install this elsewhere?
                Arguments = cmd,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            var process = new Process
            {
                StartInfo = startInfo
            };

            process.Start();
            string outputString = process.StandardOutput.ReadToEnd();
            string errorString = process.StandardError.ReadToEnd();
            process.WaitForExit();
            process.Close();
        }

        public IEnumerable<SubtitleFile> FindSubtitles(string source)
        {
            string args = $"-i \"{source}\"";
            Process proc = new Process();
            proc.StartInfo.FileName = @"c:\ffmpeg\ffmpeg.exe";
            proc.StartInfo.Arguments = args;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.UseShellExecute = false;
            if (!proc.Start())
            {
                Console.WriteLine("Error starting");
            }

            // EXPECTING WHOLE BLOCK
            string outputString = proc.StandardError.ReadToEnd();

            // EXPECTING EACH LINE OF THE OUTPUT
            string[] outpuyByLine = outputString.Trim().Split(new char[] { '\n' });

            // 
            var outputFiltered = outpuyByLine.Where(s => s.Contains("Stream #0") && s.Contains("Subtitle: "));

            var regexPattern = @"^(.*?)#(0:\d+)(\(([a-zA-Z]+)\))?: Subtitle: ([a-zA-Z]+)";
            var languages = outputFiltered.Select(s => Regex.Match(s, regexPattern));

            foreach (var match in languages)
            {
                var stream = match.Groups[2].Value;
                var language = match.Groups[4].Value;
                var format = match.Groups[5].Value;
                var fileName = $"{stream.Replace("0:", "")}_{language}";
                var fileInfo = new SubtitleFile() // TODO - Debug with Re-Zero
                {
                    SourceFileFullName = source,
                    OriginalFormat = format,
                    OutputFileName = fileName,
                    StreamId = stream,
                    Language = language ?? "und"
                };
                yield return fileInfo;
            }

            proc.WaitForExit();
            proc.Close();
        }

        public void ExtractSubtitles(SubtitleFile file, string outputFolder, bool convertToVtt = true)
        {
            var outputFileWithExtension = $"{file.OutputFileName}.{file.OriginalFormat}";
            if(file.OriginalFormat == "subrip")
                outputFileWithExtension = $"{file.OutputFileName}.srt";
            if (convertToVtt)
                outputFileWithExtension = $"{file.OutputFileName}.vtt";

            var outputFileFullName = Path.Combine(outputFolder, outputFileWithExtension);

            string args = $"-y -i \"{file.SourceFileFullName}\" -map {file.StreamId} \"{outputFileFullName}\"";
            Process proc = new Process();
            proc.StartInfo.FileName = @"c:\ffmpeg\ffmpeg.exe";
            proc.StartInfo.Arguments = args;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            if (!proc.Start())
            {
                Console.WriteLine("Error starting");
            }

            string outputString = proc.StandardOutput.ReadToEnd();
            string errorString = proc.StandardError.ReadToEnd();

            proc.WaitForExit();
            proc.Close();
        }
    }
}
