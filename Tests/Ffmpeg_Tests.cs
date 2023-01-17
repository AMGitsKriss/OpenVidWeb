using CatalogManager.Metadata;
using NUnit.Framework;
using System.Linq;

namespace Tests
{

    class Ffmpeg_Tests
    {
        [Test]
        public void Extract()
        {
            var subtitleExtractor = new FFMpegStrategy();

            var fileWithoutExt = "_CoalGuys_ K-ON S2 - 25 _Extra_ _0861F488_";
            var folder = @$"Z:\inetpub\wwwkrissflix\wwwroot\import\04_shaka_packager\{fileWithoutExt}\";
            var inputFile = @$"{folder}{fileWithoutExt}_720.mp4";

            var subtitles = subtitleExtractor.FindSubtitles(inputFile).ToList();
        }

        [Test]
        [TestCase(@"Z:\inetpub\wwwkrissflix\wwwroot\import\02_queued\The_Prince_of_Egypt.mkv", 1)]
        [TestCase(@"C:\handdbrakecli\720_src.mkv", 1)]
        [TestCase(@"C:\handbrakecli\kon.mkv", 1)]
        public void Parse(string inputFile, int expectedCount)
        {
            var subtitleExtractor = new FFMpegStrategy();

            var folder = @$"P:\kvbx_virtual\subtitles\56\565\";

            var subtitles = subtitleExtractor.FindSubtitles(inputFile).ToList();

            subtitleExtractor.ExtractSubtitles(subtitles[0], folder, false);

            Assert.That(subtitles.Count() == expectedCount);
        }
    }
}
