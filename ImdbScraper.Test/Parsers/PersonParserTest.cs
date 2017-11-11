using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImdbScraper.Parsers;
using ImdbScraper.Models;

namespace ImdbScraper.Test.Parsers
{
    [TestClass]
    public class PersonParserTest : BaseFileTest
    {
        private readonly IParser<Person> _parser = new PersonParser();

        [TestMethod]
        public async Task Imdb_Parsers_CompletePerson()
        {
            string html = GetTestFile("CompletePerson.html");
            Person result = await _parser.ParseAsync(html);

            result.Name.ShouldBeEquivalentTo("Benedict Cumberbatch");
            result.Id.ShouldBeEquivalentTo("nm1212722");
            result.Adult.Should().BeFalse("it is always false");
            result.Poster.Should().NotBeNullOrWhiteSpace();
            result.BirthDay.Should().HaveValue();
            result.BirthDay.ShouldBeEquivalentTo(new DateTime(1976, 7, 19));
            result.BirthPlace.ShouldBeEquivalentTo("Hammersmith, London, England, UK");
            result.DeathDay.Should().NotHaveValue();
            result.DeathPlace.Should().BeNullOrWhiteSpace();
            result.DeathCause.Should().BeNullOrWhiteSpace();
            result.KnownAs.Should().NotBeNull();
            result.KnownAs.Should().HaveCount(2);
            result.KnownAs[0].ShouldBeEquivalentTo("Benedict Timothy Carlton Cumberbatch");
            result.KnownAs[1].ShouldBeEquivalentTo("Ben");
            result.Credits.Cast.Should().BeEmpty("it isn't being parsed");
            result.Credits.Crew.Should().BeEmpty("it isn't being parsed");
            result.Biography.Should().NotBeNullOrWhiteSpace();
            result.Biography.ShouldBeEquivalentTo("Benedict Timothy Carlton Cumberbatch was born and "
                + "raised in London, England. His parents, Wanda Ventham and Timothy Carlton (Timothy "
                + "Carlton Congdon Cumberbatch), are both actors. He is a grandson of submarine "
                + "commander Henry Carlton Cumberbatch, and a great-grandson of diplomat Henry Arnold "
                + "Cumberbatch CMG. Cumberbatch attended Brambletye School and Harrow School. Whilst " 
                + "at Harrow, he had an arts scholarship and painted large oil canvases. It's also "
                + "where he began acting. After he finished school, he took a year off to volunteer as "
                + "an English teacher in a Tibetan monastery in Darjeeling, India. On his return, he "
                + "studied drama at Manchester University. He continued his training as an actor at the "
                + "London Academy of Music and Dramatic Art graduating with an M.A. in Classical Acting. "
                + "By the time he had completed his studies, he already had an agent.\n\n"
                + "Cumberbatch has worked in theatre, television, film and radio. His breakthrough on the "
                + "big screen came in 2004 when he portrayed Stephen Hawking in the television movie " 
                + "Hawking (2004). In 2010, he became a household name as Sherlock Holmes on the British "
                + "television series Sherlock (2010). In 2011, he appeared in two Oscar-nominated films - "
                + "War Horse (2011) and Tinker Tailor Soldier Spy (2011). He followed this with acclaimed "
                + "roles in the science fiction fiction film Star Trek Into Darkness (2013), the Oscar-" 
                + "winning drama 12 Years a Slave (2013), The Fifth Estate (2013) and August: Osage County "
                + "(2013). In 2014, he portrayed Alan Turing in The Imitation Game (2014) which earned him "
                + "a Golden Globe, Screen Actors Guild Award, British Academy of Film and Television Arts " 
                + "and an Academy Award nominations for Best Actor in a Leading Role.\n\n"
                + "Cumberbatch was appointed Commander of the Order of the British Empire (CBE) by Queen " 
                + "Elizabeth II in the 2015 Birthday Honours for his services to the performing arts and to "
                + "charity.\n\n"
                + "Cumberbatch's engagement to theatre and opera director Sophie Hunter, whom he has known "
                + "for 17 years, was announced in the \"Forthcoming Marriages\" section of The Times "
                + "newspaper on November 5, 2014. On February 14, 2015, the couple married at the 12th "
                + "century Church of St. Peter and St. Paul on the Isle of Wight followed by a reception at "
                + "Mottistone Manor. They have a son, Christopher Carlton (b. 2015).");
        }

        [TestMethod]
        public async Task Imdb_Parsers_DeadPerson()
        {
            string html = GetTestFile("DeadPerson.html");
            Person result = await _parser.ParseAsync(html);

            result.Name.ShouldBeEquivalentTo("Heath Ledger");
            result.Id.ShouldBeEquivalentTo("nm0005132");
            result.Adult.Should().BeFalse("it is always false");
            result.Poster.Should().NotBeNullOrWhiteSpace();
            result.BirthDay.Should().HaveValue();
            result.BirthDay.ShouldBeEquivalentTo(new DateTime(1979, 4, 4));
            result.BirthPlace.ShouldBeEquivalentTo("Perth, Western Australia, Australia");
            result.DeathDay.Should().HaveValue();
            result.DeathDay.ShouldBeEquivalentTo(new DateTime(2008, 1, 22));
            result.DeathPlace.Should().NotBeNullOrWhiteSpace();
            result.DeathPlace.ShouldBeEquivalentTo("Manhattan, New York City, New York, USA");
            result.DeathCause.Should().NotBeNullOrWhiteSpace();
            result.DeathCause.ShouldBeEquivalentTo("accidental overdose of prescription drugs");
            result.KnownAs.Should().NotBeNull();
            result.KnownAs.Should().HaveCount(2);
            result.KnownAs[0].ShouldBeEquivalentTo("Heath Andrew Ledger");
            result.KnownAs[1].ShouldBeEquivalentTo("Heathy");
            result.Credits.Cast.Should().BeEmpty("it isn't being parsed");
            result.Credits.Crew.Should().BeEmpty("it isn't being parsed");
            result.Biography.Should().NotBeNullOrWhiteSpace();
        }

        [TestMethod]
        [Ignore("No suitable IMDB page found")]
        public async Task Imdb_Parsers_PersonWithoutPoster()
        {
            string html = GetTestFile("PersonWithoutPoster.html");
            Person result = await _parser.ParseAsync(html);
        }
    }
}
