using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImdbScraper.Parsers;
using ImdbScraper.Models;

namespace ImdbScraper.Test.Imdb.Parsers
{
    [TestClass]
    public class SearchResultsParserTest : BaseFileTest
    {
        private readonly IParser<IEnumerable<SearchResult>> _parser = new SearchResultsParser();

        [TestMethod]
        public async Task Imdb_Parsers_Search()
        {
            string json = GetTestFile("Search.json");
            SearchResult[] result = (await _parser.ParseAsync(json)).ToArray();

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(8);

            result[0].Id.ShouldBeEquivalentTo("tt1355644");
            result[0].Name.ShouldBeEquivalentTo("Passengers");
            result[0].Details.ShouldBeEquivalentTo("Jennifer Lawrence, Chris Pratt");
            result[0].Type.ShouldBeEquivalentTo("feature");
            result[0].Image.ShouldBeEquivalentTo("https://images-na.ssl-images-amazon.com/images/M/MV5BMTk4MjU3MDIzOF5BMl5BanBnXkFtZTgwMjM2MzY2MDI@._V1_.jpg");
            result[0].Year.ShouldBeEquivalentTo(2016);

            result[4].Id.ShouldBeEquivalentTo("nm0050959");
            result[4].Name.ShouldBeEquivalentTo("Pedro Pascal");
            result[4].Details.ShouldBeEquivalentTo("Actor, Narcos (2015)");
            result[4].Type.ShouldBeEquivalentTo("person");
            result[4].Image.ShouldBeEquivalentTo("https://images-na.ssl-images-amazon.com/images/M/MV5BMzc3OTQ0ODEwMF5BMl5BanBnXkFtZTgwOTgzODIzMTE@._V1_.jpg");
            result[4].Year.Should().NotHaveValue();
        }
    }
}
