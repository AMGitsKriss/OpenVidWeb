using System;
using System.IO;
using System.Text.RegularExpressions;

namespace CatalogManager.Helpers
{
    public static class SrtHelper
    {
        private static readonly Regex _rgxCueID = new Regex(@"^\d+$");
        private static readonly Regex _rgxTimeFrame = new Regex(@"(\d\d:\d\d:\d\d(?:[,.]\d\d\d)?) --> (\d\d:\d\d:\d\d(?:[,.]\d\d\d)?)");

        public static void ConvertSrtToVtt(byte[] file, string outputFile)
        {
            using (var srtReader = new StreamReader(new MemoryStream(file)))
            using (var vttWriter = new StreamWriter(outputFile.Replace(".srt", ".vtt")))
            {
                vttWriter.WriteLine("WEBVTT"); // Starting line for the WebVTT files
                vttWriter.WriteLine("");

                string srtLine;
                while ((srtLine = srtReader.ReadLine()) != null)
                {
                    if (_rgxCueID.IsMatch(srtLine)) // Ignore cue ID number lines
                    {
                        continue;
                    }

                    Match match = _rgxTimeFrame.Match(srtLine);
                    if (match.Success) // Format the time frame to VTT format (and handle offset)
                    {
                        var startTime = TimeSpan.Parse(match.Groups[1].Value.Replace(',', '.'));
                        var endTime = TimeSpan.Parse(match.Groups[2].Value.Replace(',', '.'));

                        srtLine =
                            startTime.ToString(@"hh\:mm\:ss\.fff") +
                            " --> " +
                            endTime.ToString(@"hh\:mm\:ss\.fff");
                    }

                    vttWriter.WriteLine(srtLine);
                }
            }
        }
    }
}
