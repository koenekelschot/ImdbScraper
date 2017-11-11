using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImdbScraper.Parsers;
using ImdbScraper.Models;

namespace ImdbScraper.Test.Parsers
{
    [TestClass]
    public class SimilarTitlesParserTest : BaseFileTest
    {
        private readonly IParser<IEnumerable<Title>> _parser = new SimilarTitlesParser();

        [TestMethod]
        public async Task Imdb_Parsers_SimilarTitles()
        {
            string html = GetTestFile("SimilarTitles.html");
            Title[] result = (await _parser.ParseAsync(html)).ToArray();

            result.Should().NotBeNull();
            result.Should().HaveCount(12);

            result[0].Id.ShouldBeEquivalentTo("tt3498820");
            result[0].Name.ShouldBeEquivalentTo("Captain America: Civil War");
            result[0].Genres.Should().HaveCount(3);
            result[0].Genres[0].Name.ShouldBeEquivalentTo("Action");
            result[0].Genres[1].Name.ShouldBeEquivalentTo("Adventure");
            result[0].Genres[2].Name.ShouldBeEquivalentTo("Sci-Fi");
            result[0].Overview.ShouldBeEquivalentTo("Political interference in the Avengers' activities causes "
                + "a rift between former allies Captain America and Iron Man.");
            result[0].ReleaseDate.ShouldBeEquivalentTo(new DateTime(2016, 1, 1));

            result[11].Id.ShouldBeEquivalentTo("tt5178678");
            result[11].Name.ShouldBeEquivalentTo("Shoot the Messenger");
            result[11].Genres.Should().HaveCount(3);
            result[11].Genres[0].Name.ShouldBeEquivalentTo("Crime");
            result[11].Genres[1].Name.ShouldBeEquivalentTo("Drama");
            result[11].Genres[2].Name.ShouldBeEquivalentTo("Thriller");
            result[11].Overview.ShouldBeEquivalentTo("A young journalist who, while working on her first murder "
                + "case, becomes embroiled in a web of urban gangs, the political class, corporate power-"
                + "brokers and the police.");
            result[11].ReleaseDate.ShouldBeEquivalentTo(new DateTime(2016, 1, 1));
        }
    }
}
