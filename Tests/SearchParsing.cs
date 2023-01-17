using NUnit.Framework;
using System.Linq;
using System.Text.RegularExpressions;
using VideoHandler;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("tag:testtag", 1)]
        [TestCase("tah:testtag", 0)]
        [TestCase("-tag:testtag", 1)]
        [TestCase("-tah:testtag", 0)]
        [TestCase("-tag:testtag tah:testtag", 1)]
        [TestCase("-tag:testtag tag:food", 2)]
        [TestCase("-tag:testtag tag:food minduration:0:1:0", 3)]
        [TestCase("-tag:testtag tag:food minduration:0:1:0 order:size", 4)]
        public void SearchParsingTest(string searchString, int expectedCount)
        {
            SearchManager service = new SearchManager(null, null);
            var searchParams = service.MapSearchQueryToParameters(searchString);
            Assert.AreEqual(expectedCount, searchParams.Count());
        }

        [TestCase(@"one ""another one"" (something else)", 3)]
        [TestCase(@"one ""another one"" (something else) ""something bad)", 5)]
        public void ExperimentalParsing(string query, int expected)
        {
            var test = Split.KeepWordsQuotesBrackets(query);
            Assert.That(test.Length == expected);
        }

        private static string[] SplitOne(string query)
        {
            Regex regex = new Regex(@"[ ](?=(?:[^""]*""[^""]*"")*[^""]*$)", RegexOptions.Multiline);
            string[] splits = regex.Split(query);
            return splits;
        }

        private static string[] SplitTwo(string query)
        {
            var parts = Regex.Matches(query, @"[\""].+?[\""]|[^ ]+")
            .Cast<Match>()
            .Select(m => m.Value);

            return parts.ToArray();
        }
    }

    public static class Split
    {
        public static string[] KeepWordsQuotesBrackets(string query)
        {
            var parts = Regex.Matches(query, @"[\""].+?[\""]|[(].+?[)]|[^ ]+")
            .Cast<Match>()
            .Select(m => m.Value);

            return parts.ToArray();
        }
    }
}