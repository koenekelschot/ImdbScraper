using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImdbScraper.Parsers;
using ImdbScraper.Models;

namespace ImdbScraper.Test.Parsers
{
    [TestClass]
    public class SeasonParserTest : BaseFileTest
    {
        private readonly IParser<Season> _parser = new SeasonParser();

        [TestMethod]
        public async Task Imdb_Parsers_CompleteSeason()
        {
            string html = GetTestFile("CompleteSeason.html");
            Season result = await _parser.ParseAsync(html);

            result.Name.ShouldBeEquivalentTo("Breaking Bad");
            result.AirDate.Should().HaveValue();
            result.AirDate.Should().Be(new DateTime(2008, 1, 20));
            result.Poster.Should().NotBeNullOrWhiteSpace();
            result.SeasonNumber.Should().Be(1);
            result.Episodes.Should().NotBeNullOrEmpty();
            result.Episodes.Should().HaveCount(7);

            var episode = result.Episodes[0];
            episode.Id.Should().NotBeNullOrWhiteSpace();
            episode.Id.ShouldBeEquivalentTo("tt0959621");
            episode.Name.ShouldBeEquivalentTo("Pilot");
            episode.Overview.ShouldBeEquivalentTo("Diagnosed with terminal lung cancer, chemistry "
                + "teacher Walter White teams up with his former student, Jesse Pinkman, to cook "
                + "and sell crystal meth.");
            episode.Poster.Should().NotBeNullOrWhiteSpace();
            episode.AirDate.Should().HaveValue();
            episode.AirDate.Should().Be(new DateTime(2008, 1, 20));
            episode.SeasonNumber.Should().Be(1);
            episode.EpisodeNumber.Should().Be(1);
            episode.VoteAverage.Should().NotHaveValue();
            episode.VoteCount.Should().NotHaveValue();

            episode = result.Episodes[6];
            episode.Id.Should().NotBeNullOrWhiteSpace();
            episode.Id.ShouldBeEquivalentTo("tt1054729");
            episode.Name.ShouldBeEquivalentTo("A No-Rough-Stuff-Type Deal");
            episode.Overview.ShouldBeEquivalentTo("Walt and Jesse try to up their game by making "
                + "more of the crystal every week for Tuco. Unfortunately, some of the ingredients "
                + "they need are not easy to find. Meanwhile, Skyler realizes that her sister is a "
                + "shoplifter.");
            episode.Poster.Should().NotBeNullOrWhiteSpace();
            episode.AirDate.Should().HaveValue();
            episode.AirDate.Should().Be(new DateTime(2008, 3, 9));
            episode.SeasonNumber.Should().Be(1);
            episode.EpisodeNumber.Should().Be(7);
            episode.VoteAverage.Should().NotHaveValue();
            episode.VoteCount.Should().NotHaveValue();
        }

        [TestMethod]
        public async Task Imdb_Parsers_InconsistentEpisodeDates()
        {
            string html = GetTestFile("InconsistentEpisodeDates.html");
            Season result = await _parser.ParseAsync(html);

            result.Name.ShouldBeEquivalentTo("Sherlock");
            result.AirDate.Should().HaveValue();
            result.AirDate.Should().Be(new DateTime(2016, 1, 1));
            result.Poster.Should().NotBeNullOrWhiteSpace();
            result.SeasonNumber.Should().Be(4);
            result.Episodes.Should().NotBeNullOrEmpty();
            result.Episodes.Should().HaveCount(4);

            //1 Jan. 2016
            result.Episodes[0].AirDate.Should().HaveValue();
            result.Episodes[0].AirDate.Should().Be(new DateTime(2016, 1, 1));
            result.Episodes[0].AirDate.ShouldBeEquivalentTo(result.AirDate);

            //2017
            result.Episodes[1].AirDate.Should().HaveValue();
            result.Episodes[1].AirDate.Should().Be(new DateTime(2017, 1, 1));

            //Jan. 2017
            result.Episodes[2].AirDate.Should().HaveValue();
            result.Episodes[2].AirDate.Should().Be(new DateTime(2017, 1, 1));

            //Jan. 2017
            result.Episodes[3].AirDate.Should().HaveValue();
            result.Episodes[3].AirDate.Should().Be(new DateTime(2017, 1, 1));
        }

        [TestMethod]
        public async Task Imdb_Parsers_ZeroBasedEpisodeNumbering()
        {
            string html = GetTestFile("ZeroBasedEpisodeNumbering.html");
            Season result = await _parser.ParseAsync(html);

            result.Episodes.Should().NotBeNullOrEmpty();
            result.Episodes.Should().HaveCount(4);
            result.Episodes[0].EpisodeNumber.Should().NotBe(1, "Since the first episode is 0.");
            result.Episodes[0].EpisodeNumber.Should().Be(0, "Since the first episode is 0.");
            result.Episodes[3].EpisodeNumber.Should().NotBe(4, "Since the last episode is 3.");
            result.Episodes[3].EpisodeNumber.Should().Be(3, "Since the last episode is 3.");
        }

        [TestMethod]
        public async Task Imdb_Parsers_NoEpisodePosters()
        {
            string html = GetTestFile("NoEpisodePosters.html");
            Season result = await _parser.ParseAsync(html);

            result.Episodes.Should().NotBeNullOrEmpty();
            result.Episodes.Should().HaveCount(10);
            result.Episodes[0].Poster.Should().BeNullOrWhiteSpace();
            result.Episodes[1].Poster.Should().BeNullOrWhiteSpace();
            result.Episodes[2].Poster.Should().BeNullOrWhiteSpace();
            result.Episodes[3].Poster.Should().BeNullOrWhiteSpace();
            result.Episodes[4].Poster.Should().BeNullOrWhiteSpace();
            result.Episodes[5].Poster.Should().BeNullOrWhiteSpace();
            result.Episodes[6].Poster.Should().BeNullOrWhiteSpace();
            result.Episodes[7].Poster.Should().BeNullOrWhiteSpace();
            result.Episodes[8].Poster.Should().BeNullOrWhiteSpace();
            result.Episodes[9].Poster.Should().BeNullOrWhiteSpace();
        }
    }
}
