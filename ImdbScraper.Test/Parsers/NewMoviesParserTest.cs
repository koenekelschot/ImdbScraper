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
    public class NewMoviesParserTest : BaseFileTest
    {
        private readonly IParser<IEnumerable<Movie>> _parser = new NewMoviesParser();

        [TestMethod]
        public async Task Imdb_Parsers_InTheaters()
        {
            string html = GetTestFile("InTheaters.html");
            Movie[] result = (await _parser.ParseAsync(html)).ToArray();

            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().HaveCount(16);

            result[0].Id.ShouldBeEquivalentTo("tt1619029");
            result[0].Name.ShouldBeEquivalentTo("Jackie");
            result[0].Poster.Should().NotBeNullOrWhiteSpace();
            result[0].ReleaseDate.Should().HaveValue();
            result[0].ReleaseDate.ShouldBeEquivalentTo(new DateTime(2016, 1, 1));
            result[0].Runtime.ShouldBeEquivalentTo(99);
            result[0].Genres.Should().NotBeNullOrEmpty();
            result[0].Genres.Should().HaveCount(2);
            result[0].Genres[0].Name.ShouldBeEquivalentTo("Biography");
            result[0].Genres[1].Name.ShouldBeEquivalentTo("Drama");
            result[0].Overview.ShouldBeEquivalentTo("Following the assassination of President John F. Kennedy, "
                + "First Lady Jacqueline Kennedy fights through grief and trauma to regain her faith, console "
                + "her children, and define her husband's historic legacy.");
            result[0].Director.Should().NotBeNull();
            result[0].Director.Id.ShouldBeEquivalentTo("nm1883257");
            result[0].Director.Name.ShouldBeEquivalentTo("Pablo Larraín");
            result[0].Credits.Should().NotBeNull();
            result[0].Credits.Cast.Should().NotBeEmpty();
            result[0].Credits.Cast.Should().HaveCount(4);
            result[0].Credits.Cast[0].Name.ShouldBeEquivalentTo("Natalie Portman");
            result[0].Credits.Cast[0].Id.ShouldBeEquivalentTo("nm0000204");
            result[0].Credits.Cast[0].Character.Should().BeNull();
            result[0].Credits.Cast[1].Name.ShouldBeEquivalentTo("Peter Sarsgaard");
            result[0].Credits.Cast[1].Id.ShouldBeEquivalentTo("nm0765597");
            result[0].Credits.Cast[1].Character.Should().BeNull();
            result[0].Credits.Cast[2].Name.ShouldBeEquivalentTo("Greta Gerwig");
            result[0].Credits.Cast[2].Id.ShouldBeEquivalentTo("nm1950086");
            result[0].Credits.Cast[2].Character.Should().BeNull();
            result[0].Credits.Cast[3].Name.ShouldBeEquivalentTo("Billy Crudup");
            result[0].Credits.Cast[3].Id.ShouldBeEquivalentTo("nm0001082");
            result[0].Credits.Cast[3].Character.Should().BeNull();
            result[0].Credits.Crew.Should().BeEmpty();

            result[6].Id.ShouldBeEquivalentTo("tt3521164");
            result[6].Name.ShouldBeEquivalentTo("Moana");
            result[6].Poster.Should().NotBeNullOrWhiteSpace();
            result[6].ReleaseDate.Should().HaveValue();
            result[6].ReleaseDate.ShouldBeEquivalentTo(new DateTime(2016, 1, 1));
            result[6].Runtime.ShouldBeEquivalentTo(103);
            result[6].Genres.Should().NotBeNullOrEmpty();
            result[6].Genres.Should().HaveCount(6);
            result[6].Genres[0].Name.ShouldBeEquivalentTo("Animation");
            result[6].Genres[1].Name.ShouldBeEquivalentTo("Adventure");
            result[6].Genres[2].Name.ShouldBeEquivalentTo("Comedy");
            result[6].Genres[3].Name.ShouldBeEquivalentTo("Family");
            result[6].Genres[4].Name.ShouldBeEquivalentTo("Fantasy");
            result[6].Genres[5].Name.ShouldBeEquivalentTo("Musical");
            result[6].Overview.ShouldBeEquivalentTo("In Ancient Polynesia, when a terrible curse incurred by "
                + "Maui reaches an impetuous Chieftain's daughter's island, she answers the Ocean's call to "
                + "seek out the demigod to set things right.");
            result[6].Director.Should().NotBeNull();
            result[6].Director.Id.ShouldBeEquivalentTo("nm0166256");
            result[6].Director.Name.ShouldBeEquivalentTo("Ron Clements");
            result[6].Credits.Should().NotBeNull();
            result[6].Credits.Cast.Should().NotBeEmpty();
            result[6].Credits.Cast.Should().HaveCount(4);
            result[6].Credits.Cast[0].Name.ShouldBeEquivalentTo("Auli'i Cravalho");
            result[6].Credits.Cast[0].Id.ShouldBeEquivalentTo("nm7635388");
            result[6].Credits.Cast[0].Character.Should().BeNull();
            result[6].Credits.Cast[1].Name.ShouldBeEquivalentTo("Dwayne Johnson");
            result[6].Credits.Cast[1].Id.ShouldBeEquivalentTo("nm0425005");
            result[6].Credits.Cast[1].Character.Should().BeNull();
            result[6].Credits.Cast[2].Name.ShouldBeEquivalentTo("Rachel House");
            result[6].Credits.Cast[2].Id.ShouldBeEquivalentTo("nm1344302");
            result[6].Credits.Cast[2].Character.Should().BeNull();
            result[6].Credits.Cast[3].Name.ShouldBeEquivalentTo("Temuera Morrison");
            result[6].Credits.Cast[3].Id.ShouldBeEquivalentTo("nm0607325");
            result[6].Credits.Cast[3].Character.Should().BeNull();
            result[6].Credits.Crew.Should().BeEmpty();
        }

        [TestMethod]
        public async Task Imdb_Parsers_ComingSoon()
        {
            string html = GetTestFile("ComingSoon.html");
            Movie[] result = (await _parser.ParseAsync(html)).ToArray();

            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().HaveCount(26);

            result[0].Id.ShouldBeEquivalentTo("tt3783958");
            result[0].Name.ShouldBeEquivalentTo("La La Land");
            result[0].Poster.Should().NotBeNullOrWhiteSpace();
            result[0].ReleaseDate.Should().HaveValue();
            result[0].ReleaseDate.ShouldBeEquivalentTo(new DateTime(2016, 1, 1));
            result[0].Runtime.ShouldBeEquivalentTo(128);
            result[0].Genres.Should().NotBeNullOrEmpty();
            result[0].Genres.Should().HaveCount(4);
            result[0].Genres[0].Name.ShouldBeEquivalentTo("Comedy");
            result[0].Genres[1].Name.ShouldBeEquivalentTo("Drama");
            result[0].Genres[2].Name.ShouldBeEquivalentTo("Musical");
            result[0].Genres[3].Name.ShouldBeEquivalentTo("Romance");
            result[0].Overview.ShouldBeEquivalentTo("A jazz pianist falls for an aspiring actress in Los Angeles.");
            result[0].Director.Should().NotBeNull();
            result[0].Director.Id.ShouldBeEquivalentTo("nm3227090");
            result[0].Director.Name.ShouldBeEquivalentTo("Damien Chazelle");
            result[0].Credits.Should().NotBeNull();
            result[0].Credits.Cast.Should().NotBeEmpty();
            result[0].Credits.Cast.Should().HaveCount(4);
            result[0].Credits.Cast[0].Name.ShouldBeEquivalentTo("Ryan Gosling");
            result[0].Credits.Cast[0].Id.ShouldBeEquivalentTo("nm0331516");
            result[0].Credits.Cast[0].Character.Should().BeNull();
            result[0].Credits.Cast[1].Name.ShouldBeEquivalentTo("Emma Stone");
            result[0].Credits.Cast[1].Id.ShouldBeEquivalentTo("nm1297015");
            result[0].Credits.Cast[1].Character.Should().BeNull();
            result[0].Credits.Cast[2].Name.ShouldBeEquivalentTo("Rosemarie DeWitt");
            result[0].Credits.Cast[2].Id.ShouldBeEquivalentTo("nm1679669");
            result[0].Credits.Cast[2].Character.Should().BeNull();
            result[0].Credits.Cast[3].Name.ShouldBeEquivalentTo("J.K. Simmons");
            result[0].Credits.Cast[3].Id.ShouldBeEquivalentTo("nm0799777");
            result[0].Credits.Cast[3].Character.Should().BeNull();
            result[0].Credits.Crew.Should().BeEmpty();

            result[4].Id.ShouldBeEquivalentTo("tt3319844");
            result[4].Name.ShouldBeEquivalentTo("Harry Benson: Shoot First");
            result[4].Poster.Should().NotBeNullOrWhiteSpace();
            result[4].ReleaseDate.Should().HaveValue();
            result[4].ReleaseDate.ShouldBeEquivalentTo(new DateTime(2016, 1, 1));
            result[4].Runtime.Should().NotHaveValue("it is optional");
            result[4].Genres.Should().NotBeNullOrEmpty();
            result[4].Genres.Should().HaveCount(1);
            result[4].Genres[0].Name.ShouldBeEquivalentTo("Documentary");
            result[4].Overview.ShouldBeEquivalentTo("What we know today about many famous musicians, politicians, and "
                + "actresses is due to the famous work of photographer Harry Benson. He captured vibrant and intimate "
                + "photos of the most famous band in history;The Beatles. His extensive portfolio grew to include "
                + "iconic photos of Muhammad Ali, Michael Jackson, and Dr. Martin Luther King. His wide-ranging work "
                + "has appeared in publications including Life, Vanity Fair and The New Yorker. Benson, now 86, is "
                + "still taking photos and has no intentions of stopping.");
            result[4].Director.Should().NotBeNull();
            result[4].Director.Id.ShouldBeEquivalentTo("nm3293337");
            result[4].Director.Name.ShouldBeEquivalentTo("Justin Bare");
            result[4].Credits.Should().NotBeNull();
            result[4].Credits.Cast.Should().NotBeEmpty();
            result[4].Credits.Cast.Should().HaveCount(4);
            result[4].Credits.Cast[0].Name.ShouldBeEquivalentTo("Harry Benson");
            result[4].Credits.Cast[0].Id.ShouldBeEquivalentTo("nm2505486");
            result[4].Credits.Cast[0].Character.Should().BeNull();
            result[4].Credits.Cast[1].Name.ShouldBeEquivalentTo("Alec Baldwin");
            result[4].Credits.Cast[1].Id.ShouldBeEquivalentTo("nm0000285");
            result[4].Credits.Cast[1].Character.Should().BeNull();
            result[4].Credits.Cast[2].Name.ShouldBeEquivalentTo("Gigi Benson");
            result[4].Credits.Cast[2].Id.ShouldBeEquivalentTo("nm6116186");
            result[4].Credits.Cast[2].Character.Should().BeNull();
            result[4].Credits.Cast[3].Name.ShouldBeEquivalentTo("Wendy Benson-Landes");
            result[4].Credits.Cast[3].Id.ShouldBeEquivalentTo("nm0072673");
            result[4].Credits.Cast[3].Character.Should().BeNull();
            result[4].Credits.Crew.Should().BeEmpty();
        }
    }
}
