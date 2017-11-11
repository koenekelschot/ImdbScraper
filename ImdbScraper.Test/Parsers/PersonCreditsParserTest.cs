using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImdbScraper.Parsers;
using ImdbScraper.Models;

namespace ImdbScraper.Test.Parsers
{
    [TestClass]
    public class PersonCreditsParserTest : BaseFileTest
    {
        private readonly IParser<PersonCredits> _parser = new PersonCreditsParser();

        [TestMethod]
        public async Task Imdb_Parsers_PersonCredits()
        {
            string html = GetTestFile("PersonCredits.html");
            PersonCredits result = await _parser.ParseAsync(html);

            result.Cast.Should().NotBeNull();
            result.Cast.Should().HaveCount(75);
            result.Cast[0].Id.ShouldBeEquivalentTo("tt2084970");
            result.Cast[0].Title.ShouldBeEquivalentTo("The Imitation Game");
            result.Cast[0].Character.ShouldBeEquivalentTo("Alan Turing");
            result.Cast[0].ReleaseDate.ShouldBeEquivalentTo(new DateTime(2014, 1, 1));
            result.Cast[0].Poster.Should().NotBeNullOrWhiteSpace();
            result.Cast[0].Adult.Should().BeFalse("It isn't parsed");
            result.Cast[4].Id.ShouldBeEquivalentTo("tt1475582");
            result.Cast[4].Title.ShouldBeEquivalentTo("Sherlock");
            result.Cast[4].Character.ShouldBeEquivalentTo("Sherlock Holmes");
            result.Cast[4].ReleaseDate.ShouldBeEquivalentTo(new DateTime(2010, 1, 1));
            result.Cast[4].Poster.Should().NotBeNullOrWhiteSpace();
            result.Cast[4].Adult.Should().BeFalse("It isn't parsed");

            result.Crew.Should().NotBeNull();
            result.Crew.Should().HaveCount(0, "it isn't parsed");
        }
    }
}
