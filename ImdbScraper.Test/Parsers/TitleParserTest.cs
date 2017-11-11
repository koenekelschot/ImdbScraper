using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImdbScraper.Parsers;
using ImdbScraper.Models;

namespace ImdbScraper.Test.Parsers
{
    [TestClass]
    public class TitleParserTest : BaseFileTest
    {
        private readonly IParser<Title> _parser = new TitleParser();

        [TestMethod]
        public async Task Imdb_Parsers_CompleteMovie()
        {
            string html = GetTestFile("CompleteMovie.html");
            Movie result = (Movie) await _parser.ParseAsync(html);

            result.Id.ShouldBeEquivalentTo("tt2241351");
            result.Name.ShouldBeEquivalentTo("Money Monster");
            result.Overview.Should().NotBeNullOrWhiteSpace();
            result.Overview.ShouldBeEquivalentTo("In the real-time, high stakes thriller Money " 
                + "Monster, George Clooney and Julia Roberts star as financial TV host Lee Gates " 
                + "and his producer Patty, who are put in an extreme situation when an irate " 
                + "investor who has lost everything (Jack O'Connell) forcefully takes over their " 
                + "studio. During a tense standoff broadcast to millions on live TV, Lee and Patty " 
                + "must work furiously against the clock to unravel the mystery behind a conspiracy " 
                + "at the heart of today's fast-paced, high-tech global markets.");
            result.Poster.Should().NotBeNullOrWhiteSpace();
            result.Genres.Should().NotBeNullOrEmpty();
            result.Genres.Should().HaveCount(3);
            result.Genres[0].Name.ShouldBeEquivalentTo("Crime");
            result.Genres[1].Name.ShouldBeEquivalentTo("Drama");
            result.Genres[2].Name.ShouldBeEquivalentTo("Thriller");
            result.Keywords.Should().NotBeNullOrEmpty();
            result.Keywords.Should().HaveCount(5);
            result.Keywords[0].Name.ShouldBeEquivalentTo("money");
            result.Keywords[1].Name.ShouldBeEquivalentTo("hostage");
            result.Keywords[2].Name.ShouldBeEquivalentTo("death");
            result.Keywords[3].Name.ShouldBeEquivalentTo("shot in the heart");
            result.Keywords[4].Name.ShouldBeEquivalentTo("shot in the chest");
            result.VoteAverage.ShouldBeEquivalentTo(6.6);
            result.VoteCount.ShouldBeEquivalentTo(47386);
            result.Runtime.ShouldBeEquivalentTo(98);

            result.TagLine.Should().NotBeNullOrWhiteSpace();
            result.TagLine.ShouldBeEquivalentTo("Anyone who can get out, get out right now.");
            result.Adult.Should().BeFalse();
            result.Director.Should().NotBeNull();
            result.Director.Name.ShouldBeEquivalentTo("Jodie Foster");
            result.Director.Id.ShouldBeEquivalentTo("nm0000149");
            result.ReleaseDate.Should().HaveValue();
            result.ReleaseDate.ShouldBeEquivalentTo(new DateTime(2016, 5, 13));
        }

        [TestMethod]
        public async Task Imdb_Parsers_IncompleteMovie()
        {
            string html = GetTestFile("IncompleteMovie.html");
            Movie result = (Movie) await _parser.ParseAsync(html);

            result.Id.ShouldBeEquivalentTo("tt1734493");
            result.Name.ShouldBeEquivalentTo("Unlocked");
            result.Overview.Should().NotBeNullOrWhiteSpace();
            result.Overview.ShouldBeEquivalentTo("A CIA interrogator is lured into a ruse that "
                + "puts London at risk of a biological attack.");
            result.Poster.Should().BeNull("It is not on the page");
            result.Genres.Should().NotBeNullOrEmpty();
            result.Genres.Should().HaveCount(2);
            result.Genres[0].Name.ShouldBeEquivalentTo("Action");
            result.Genres[1].Name.ShouldBeEquivalentTo("Thriller");
            result.Keywords.Should().BeEmpty();
            result.VoteAverage.Should().NotHaveValue();
            result.VoteCount.Should().NotHaveValue();
            result.Runtime.Should().NotHaveValue();

            result.TagLine.Should().BeNullOrWhiteSpace();
            result.Adult.Should().BeFalse();
            result.Director.Should().NotBeNull();
            result.Director.Name.ShouldBeEquivalentTo("Michael Apted");
            result.Director.Id.ShouldBeEquivalentTo("nm0000776");
            result.ReleaseDate.Should().HaveValue();
            result.ReleaseDate.ShouldBeEquivalentTo(new DateTime(2017, 3, 17));
        }

        [TestMethod]
        public async Task Imdb_Parsers_CompleteShow()
        {
            string html = GetTestFile("CompleteShow.html");
            Show result = (Show) await _parser.ParseAsync(html);

            result.Id.ShouldBeEquivalentTo("tt1520211");
            result.Name.ShouldBeEquivalentTo("The Walking Dead");
            result.Overview.Should().NotBeNullOrWhiteSpace();
            result.Overview.ShouldBeEquivalentTo("Rick Grimes is a former Sheriff's deputy who has " 
                + "been in a coma for several months after being shot while on duty. When he " 
                + "awakens he discovers that the world has been ravished by a zombie epidemic of " 
                + "apocalyptic proportions, and that he seems to be the only person still alive. " 
                + "After returning home to discover his wife and son missing, he runs into survivors " 
                + "Morgan and his son; who teach him the ropes of the new world. He then heads for " 
                + "Atlanta to search for his family. Narrowly escaping death at the hands of the " 
                + "zombies on arrival in Atlanta he is aided by another survivor, Glenn, who takes " 
                + "Rick to a camp outside the town. There Rick finds his wife Lori and his son, " 
                + "Carl, along with his partner/best friend Shane and a small group of survivors who " 
                + "struggle to fend off the zombie hordes; as well as competing with other " 
                + "surviving groups who are prepared to do whatever it takes to survive in this " 
                + "harsh new world.");
            result.Poster.Should().NotBeNullOrWhiteSpace();
            result.Genres.Should().NotBeNullOrEmpty();
            result.Genres.Should().HaveCount(3);
            result.Genres[0].Name.ShouldBeEquivalentTo("Drama");
            result.Genres[1].Name.ShouldBeEquivalentTo("Horror");
            result.Genres[2].Name.ShouldBeEquivalentTo("Sci-Fi");
            result.Keywords.Should().NotBeNullOrEmpty();
            result.Keywords.Should().HaveCount(5);
            result.Keywords[0].Name.ShouldBeEquivalentTo("zombie");
            result.Keywords[1].Name.ShouldBeEquivalentTo("based on comic");
            result.Keywords[2].Name.ShouldBeEquivalentTo("survival");
            result.Keywords[3].Name.ShouldBeEquivalentTo("flesh eating zombie");
            result.Keywords[4].Name.ShouldBeEquivalentTo("undead");
            result.VoteAverage.ShouldBeEquivalentTo(8.6);
            result.VoteCount.ShouldBeEquivalentTo(649505);
            result.Runtime.ShouldBeEquivalentTo(44);

            result.CreatedBy.Should().NotBeNull();
            result.CreatedBy.Should().HaveCount(1);
            result.CreatedBy[0].Name.ShouldBeEquivalentTo("Frank Darabont");
            result.CreatedBy[0].Id.ShouldBeEquivalentTo("nm0001104");
            result.FirstAirDate.Should().HaveValue();
            result.FirstAirDate.ShouldBeEquivalentTo(new DateTime(2010, 1, 1));
            result.LastAirDate.Should().NotHaveValue();
            result.EpisodeCount.ShouldBeEquivalentTo(100);
            result.SeasonCount.ShouldBeEquivalentTo(8);
            result.Seasons.Should().BeEmpty();
        }

        [TestMethod]
        [Ignore("No suitable IMDB page found")]
        public async Task Imdb_Parsers_IncompleteShow()
        {
            string html = GetTestFile("IncompleteShow.html");
            Show result = (Show) await _parser.ParseAsync(html);
        }

        [TestMethod]
        public async Task Imdb_Parsers_CompleteEpisode()
        {
            string html = GetTestFile("CompleteEpisode.html");
            Episode result = (Episode)await _parser.ParseAsync(html);

            result.Id.ShouldBeEquivalentTo("tt0959621");
            result.Name.ShouldBeEquivalentTo("Pilot");
            result.Overview.Should().NotBeNullOrWhiteSpace();
            result.Overview.ShouldBeEquivalentTo("Just days after his 50th birthday, chemistry "
                + "teacher Walter White's life of quiet desperation completely transforms when "
                + "he's diagnosed with inoperable lung cancer. To support his pregnant wife and " 
                + "son, he partners with Jesse, a former student, to turn an old RV into a mobile "
                + "meth lab. But their first attempt at unloading their product takes a deadly "
                + "turn when Jesse introduces Walt to his unstable business associates.");
            result.Poster.Should().NotBeNullOrWhiteSpace();
            result.Genres.Should().NotBeNullOrEmpty();
            result.Genres.Should().HaveCount(3);
            result.Genres[0].Name.ShouldBeEquivalentTo("Crime");
            result.Genres[1].Name.ShouldBeEquivalentTo("Drama");
            result.Genres[2].Name.ShouldBeEquivalentTo("Thriller");
            result.Keywords.Should().NotBeNullOrEmpty();
            result.Keywords.Should().HaveCount(5);
            result.Keywords[0].Name.ShouldBeEquivalentTo("high school");
            result.Keywords[1].Name.ShouldBeEquivalentTo("pregnant");
            result.Keywords[2].Name.ShouldBeEquivalentTo("meth lab");
            result.Keywords[3].Name.ShouldBeEquivalentTo("dea");
            result.Keywords[4].Name.ShouldBeEquivalentTo("car wash");
            result.VoteAverage.ShouldBeEquivalentTo(9.0);
            result.VoteCount.ShouldBeEquivalentTo(13707);
            result.Runtime.ShouldBeEquivalentTo(58);

            result.Adult.Should().BeFalse();
            result.AirDate.Should().HaveValue();
            result.AirDate.ShouldBeEquivalentTo(new DateTime(2008, 1, 20));
            result.SeasonNumber.ShouldBeEquivalentTo(1);
            result.EpisodeNumber.ShouldBeEquivalentTo(1);
        }

        [TestMethod]
        [Ignore("No suitable IMDB page found")]
        public async Task Imdb_Parsers_IncompleteEpisode()
        {
            string html = GetTestFile("IncompleteEpisode.html");
            Episode result = (Episode)await _parser.ParseAsync(html);
        }

        [TestMethod]
        public async Task Imdb_Parsers_TitleCredits()
        {
            string html = GetTestFile("TitleCredits.html");
            Title result = await _parser.ParseAsync(html);

            result.Credits.Cast.Should().NotBeNullOrEmpty();
            result.Credits.Cast.Should().HaveCount(10);
            result.Credits.Crew.Should().NotBeNull();
            result.Credits.Crew.Should().BeEmpty("because it isn't provided by IMDB");

            result.Credits.Cast[0].Name.ShouldBeEquivalentTo("Bryan Cranston");
            result.Credits.Cast[0].Id.ShouldBeEquivalentTo("nm0186505");
            result.Credits.Cast[0].Character.ShouldBeEquivalentTo("Walter White");
            result.Credits.Cast[1].Name.ShouldBeEquivalentTo("Anna Gunn");
            result.Credits.Cast[1].Id.ShouldBeEquivalentTo("nm0348152");
            result.Credits.Cast[1].Character.ShouldBeEquivalentTo("Skyler White");
            result.Credits.Cast[2].Name.ShouldBeEquivalentTo("Aaron Paul");
            result.Credits.Cast[2].Id.ShouldBeEquivalentTo("nm0666739");
            result.Credits.Cast[2].Character.ShouldBeEquivalentTo("Jesse Pinkman");
            result.Credits.Cast[3].Name.ShouldBeEquivalentTo("Dean Norris");
            result.Credits.Cast[3].Id.ShouldBeEquivalentTo("nm0606487");
            result.Credits.Cast[3].Character.ShouldBeEquivalentTo("Hank Schrader");
            result.Credits.Cast[4].Name.ShouldBeEquivalentTo("Betsy Brandt");
            result.Credits.Cast[4].Id.ShouldBeEquivalentTo("nm1336827");
            result.Credits.Cast[4].Character.ShouldBeEquivalentTo("Marie Schrader");
            result.Credits.Cast[5].Name.ShouldBeEquivalentTo("RJ Mitte");
            result.Credits.Cast[5].Id.ShouldBeEquivalentTo("nm2666409");
            result.Credits.Cast[5].Character.ShouldBeEquivalentTo("Walter White, Jr.");
            result.Credits.Cast[6].Name.ShouldBeEquivalentTo("Bob Odenkirk");
            result.Credits.Cast[6].Id.ShouldBeEquivalentTo("nm0644022");
            result.Credits.Cast[6].Character.ShouldBeEquivalentTo("Saul Goodman");
            result.Credits.Cast[7].Name.ShouldBeEquivalentTo("Steven Michael Quezada");
            result.Credits.Cast[7].Id.ShouldBeEquivalentTo("nm2366374");
            result.Credits.Cast[7].Character.ShouldBeEquivalentTo("Steven Gomez");
            result.Credits.Cast[8].Name.ShouldBeEquivalentTo("Jonathan Banks");
            result.Credits.Cast[8].Id.ShouldBeEquivalentTo("nm0052186");
            result.Credits.Cast[8].Character.ShouldBeEquivalentTo("Mike Ehrmantraut");
            result.Credits.Cast[9].Name.ShouldBeEquivalentTo("Giancarlo Esposito");
            result.Credits.Cast[9].Id.ShouldBeEquivalentTo("nm0002064");
            result.Credits.Cast[9].Character.ShouldBeEquivalentTo("Gustavo 'Gus' Fring");
        }
    }
}
