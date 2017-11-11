using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImdbScraper.Parsers;
using ImdbScraper.Models;

namespace ImdbScraper.Test.Parsers
{
    [TestClass]
    public class MediaCreditsParserTest : BaseFileTest
    {
        private readonly IParser<MediaCredits> _parser = new MediaCreditsParser();

        [TestMethod]
        public async Task Imdb_Parsers_MediaCredits()
        {
            string html = GetTestFile("MediaCredits.html");
            MediaCredits result = await _parser.ParseAsync(html);

            result.Cast.Should().NotBeNull();
            result.Cast.Should().HaveCount(162);
            result.Cast[0].Id.ShouldBeEquivalentTo("nm0000129");
            result.Cast[0].Name.ShouldBeEquivalentTo("Tom Cruise");
            result.Cast[0].Character.ShouldBeEquivalentTo("Jack Reacher");
            result.Cast[1].Id.ShouldBeEquivalentTo("nm1130627");
            result.Cast[1].Name.ShouldBeEquivalentTo("Cobie Smulders");
            result.Cast[1].Character.ShouldBeEquivalentTo("Turner");
            result.Cast[161].Id.ShouldBeEquivalentTo("nm6813702");
            result.Cast[161].Name.ShouldBeEquivalentTo("John Phillip Yates");
            result.Cast[161].Character.ShouldBeEquivalentTo("Bus Passenger (uncredited)");

            result.Crew.Should().NotBeNull();
            result.Crew.Any(c => c.Department == "Directors").Should().BeTrue();
            result.Crew.Where(c => c.Department == "Directors").Should().HaveCount(1);
            result.Crew.First(c => c.Department == "Directors").Job.Should().BeNull("It should be ignored");
            result.Crew.Any(c => c.Department == "Writers").Should().BeTrue();
            result.Crew.Where(c => c.Department == "Writers").Should().HaveCount(4);
            result.Crew.First(c => c.Department == "Writers").Job.Should().BeNull("It should be ignored");
            result.Crew.Any(c => c.Department == "Producers").Should().BeTrue();
            result.Crew.Where(c => c.Department == "Producers").Should().HaveCount(7);
            result.Crew.First(c => c.Department == "Producers").Job.Should().NotBeNull();
        }
    }
}
