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
    public class TitleCHartParserTest : BaseFileTest
    {
        private readonly IParser<IEnumerable<Movie>> _movieParser = new TitleChartParser<Movie>();
        private readonly IParser<IEnumerable<Show>> _showParser = new TitleChartParser<Show>();

        [TestMethod]
        public async Task Imdb_Parsers_MovieTop250()
        {
            string html = GetTestFile("MovieTop250.html");
            Movie[] result = (await _movieParser.ParseAsync(html)).ToArray();

            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().HaveCount(250);

            result[0].Id.ShouldBeEquivalentTo("tt0111161");
            result[0].Name.ShouldBeEquivalentTo("The Shawshank Redemption");
            result[0].Poster.Should().NotBeNullOrWhiteSpace();
            result[0].ReleaseDate.Should().HaveValue();
            result[0].ReleaseDate.ShouldBeEquivalentTo(new DateTime(1994, 1, 1));
            result[0].VoteAverage.Should().HaveValue();
            result[0].VoteAverage.ShouldBeEquivalentTo(9.2);

            result[249].Id.ShouldBeEquivalentTo("tt1220719");
            result[249].Name.ShouldBeEquivalentTo("Ip Man");
            result[249].Poster.Should().NotBeNullOrWhiteSpace();
            result[249].ReleaseDate.Should().HaveValue();
            result[249].ReleaseDate.ShouldBeEquivalentTo(new DateTime(2008, 1, 1));
            result[249].VoteAverage.Should().HaveValue();
            result[249].VoteAverage.ShouldBeEquivalentTo(8.0);
        }

        [TestMethod]
        public async Task Imdb_Parsers_PopularMovies()
        {
            string html = GetTestFile("PopularMovies.html");
            Movie[] result = (await _movieParser.ParseAsync(html)).ToArray();

            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().HaveCount(100);

            result[0].Id.ShouldBeEquivalentTo("tt3183660");
            result[0].Name.ShouldBeEquivalentTo("Fantastic Beasts and Where to Find Them");
            result[0].Poster.Should().NotBeNullOrWhiteSpace();
            result[0].ReleaseDate.Should().HaveValue();
            result[0].ReleaseDate.ShouldBeEquivalentTo(new DateTime(2016, 1, 1));
            result[0].VoteAverage.Should().HaveValue();
            result[0].VoteAverage.ShouldBeEquivalentTo(7.8);

            result[4].Id.ShouldBeEquivalentTo("tt3748528");
            result[4].Name.ShouldBeEquivalentTo("Rogue One: A Star Wars Story");
            result[4].Poster.Should().NotBeNullOrWhiteSpace();
            result[4].ReleaseDate.Should().HaveValue();
            result[4].ReleaseDate.ShouldBeEquivalentTo(new DateTime(2016, 1, 1));
            result[4].VoteAverage.Should().NotHaveValue();

            result[99].Id.ShouldBeEquivalentTo("tt0363771");
            result[99].Name.ShouldBeEquivalentTo("The Chronicles of Narnia: The Lion, the Witch and the Wardrobe");
            result[99].Poster.Should().NotBeNullOrWhiteSpace();
            result[99].ReleaseDate.Should().HaveValue();
            result[99].ReleaseDate.ShouldBeEquivalentTo(new DateTime(2005, 1, 1));
            result[99].VoteAverage.Should().HaveValue();
            result[99].VoteAverage.ShouldBeEquivalentTo(6.9);
        }

        [TestMethod]
        public async Task Imdb_Parsers_ShowTop250()
        {
            string html = GetTestFile("ShowTop250.html");
            Show[] result = (await _showParser.ParseAsync(html)).ToArray();

            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().HaveCount(250);

            result[0].Id.ShouldBeEquivalentTo("tt5491994");
            result[0].Name.ShouldBeEquivalentTo("Planet Earth II");
            result[0].Poster.Should().NotBeNullOrWhiteSpace();
            result[0].FirstAirDate.Should().HaveValue();
            result[0].FirstAirDate.ShouldBeEquivalentTo(new DateTime(2016, 1, 1));
            result[0].VoteAverage.Should().HaveValue();
            result[0].VoteAverage.ShouldBeEquivalentTo(9.5);

            result[249].Id.ShouldBeEquivalentTo("tt0421291");
            result[249].Name.ShouldBeEquivalentTo("Avrupa Yakasi");
            result[249].Poster.Should().NotBeNullOrWhiteSpace();
            result[249].FirstAirDate.Should().HaveValue();
            result[249].FirstAirDate.ShouldBeEquivalentTo(new DateTime(2004, 1, 1));
            result[249].VoteAverage.Should().HaveValue();
            result[249].VoteAverage.ShouldBeEquivalentTo(8.3);
        }

        [TestMethod]
        public async Task Imdb_Parsers_PopularShows()
        {
            string html = GetTestFile("PopularShows.html");
            Show[] result = (await _showParser.ParseAsync(html)).ToArray();

            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().HaveCount(100);

            result[0].Id.ShouldBeEquivalentTo("tt0475784");
            result[0].Name.ShouldBeEquivalentTo("Westworld");
            result[0].Poster.Should().NotBeNullOrWhiteSpace();
            result[0].FirstAirDate.Should().HaveValue();
            result[0].FirstAirDate.ShouldBeEquivalentTo(new DateTime(2016, 1, 1));
            result[0].VoteAverage.Should().HaveValue();
            result[0].VoteAverage.ShouldBeEquivalentTo(9.2);

            result[13].Id.ShouldBeEquivalentTo("tt4834206");
            result[13].Name.ShouldBeEquivalentTo("A Series of Unfortunate Events");
            result[13].Poster.Should().NotBeNullOrWhiteSpace();
            result[13].FirstAirDate.Should().HaveValue();
            result[13].FirstAirDate.ShouldBeEquivalentTo(new DateTime(2017, 1, 1));
            result[13].VoteAverage.Should().NotHaveValue();

            result[99].Id.ShouldBeEquivalentTo("tt5396572");
            result[99].Name.ShouldBeEquivalentTo("Conviction");
            result[99].Poster.Should().NotBeNullOrWhiteSpace();
            result[99].FirstAirDate.Should().HaveValue();
            result[99].FirstAirDate.ShouldBeEquivalentTo(new DateTime(2016, 1, 1));
            result[99].VoteAverage.Should().HaveValue();
            result[99].VoteAverage.ShouldBeEquivalentTo(7.3);
        }
    }
}
