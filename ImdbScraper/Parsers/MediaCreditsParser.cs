using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using ImdbScraper.Models;

namespace ImdbScraper.Parsers
{
    public class MediaCreditsParser : IParser<MediaCredits>
    {
        private IHtmlDocument _document;
        private MediaCredits _result;

        public async Task<MediaCredits> ParseAsync(string html)
        {
            HtmlParser parser = new HtmlParser();
            _result = new MediaCredits();
            _document = await parser.ParseAsync(html);

            try
            {
                ParseCast();
                ParseCrew();

                return _result;
            }
            catch (HtmlParseException e)
            {
                throw new ParserException("Error parsing HTML for MediaCredits.", e);
            }
        }

        private void ParseCast()
        {
            var headerElement = _document.QuerySelector("h4#cast");

            if (headerElement == null)
                return;

            var tableElement = headerElement.NextElementSibling;
            var rows = tableElement.QuerySelectorAll("tr").Where(e => e.ClassName == "odd" || e.ClassName == "even");

            foreach (var row in rows)
            {
                var name = row.QuerySelector("a[itemprop='url']");
                if (name == null)
                    continue; //ignore empty rows

                var character = row.QuerySelector(".character");
                var characterName = Regex.Replace(character.TextContent, @"\s+", " ");

                _result.Cast.Add(new MediaCast
                {
                    Id = ImdbHelper.GetPersonIdFromUrl(name.GetAttribute("href")),
                    Name = name.TextContent.Trim(),
                    Character = characterName.Trim()
                });
            }
        }

        private void ParseCrew()
        {
            ParseSimpleCreditsTable("Directed by", "Directors", true);
            ParseSimpleCreditsTable("Writing Credits", "Writers", true);
            ParseSimpleCreditsTable("Produced by", "Producers");
            ParseSimpleCreditsTable("Music by", "Music by");
            ParseSimpleCreditsTable("Cinematography by", "Cinematography");
            ParseSimpleCreditsTable("Film Editing by", "Film Editing");
            ParseSimpleCreditsTable("Production Design by", "Production Design");
            ParseSimpleCreditsTable("Art Direction by", "Art Direction");
            ParseSimpleCreditsTable("Set Decoration by", "Set Decoration");
            ParseSimpleCreditsTable("Costume Design by", "Costume Design");
            ParseSimpleCreditsTable("Makeup Department", "Makeup");
            ParseSimpleCreditsTable("Production Management", "Production Management");
            ParseSimpleCreditsTable("Second Unit Director or Assistant Director", "Second Unit Director or Assistant Director");
            ParseSimpleCreditsTable("Art Department", "Art");
            ParseSimpleCreditsTable("Sound Department", "Sound");
            ParseSimpleCreditsTable("Special Effects by", "Special Effects");
            ParseSimpleCreditsTable("Visual Effects by", "Visual Effects");
            ParseSimpleCreditsTable("Stunts", "Stunts");
            ParseSimpleCreditsTable("Camera and Electrical Department", "Camera and Electrical");
            ParseSimpleCreditsTable("Casting Department", "Casting");
            ParseSimpleCreditsTable("Costume and Wardrobe Department", "Costume and Wardrobe");
            ParseSimpleCreditsTable("Editorial Department", "Editorial");
            ParseSimpleCreditsTable("Location Management", "Location Management");
            ParseSimpleCreditsTable("Music Department", "Music");
            ParseSimpleCreditsTable("Transportation Department", "Transportation");
            ParseSimpleCreditsTable("Other crew", "Other");
        }

        private void ParseSimpleCreditsTable(string headerText, string department, bool ignoreJob = false)
        {
            var headerElement =
                _document.QuerySelectorAll("#fullcredits_content h4")
                    .First(e => e.TextContent.Trim().Equals(headerText, StringComparison.OrdinalIgnoreCase));

            if (headerElement == null)
                return;

            var tableElement = headerElement.NextElementSibling;
            var rows = tableElement.QuerySelectorAll("tr");

            foreach (var row in rows)
            {
                var name = row.QuerySelector("a");
                if (name == null)
                    continue; //ignore empty rows

                var credit = row.QuerySelector(".credit");
                string job = ignoreJob ? null : credit?.TextContent.Trim();

                _result.Crew.Add(new MediaCrew
                {
                    Id = ImdbHelper.GetPersonIdFromUrl(name.GetAttribute("href")),
                    Name = name.TextContent.Trim(),
                    Department = department,
                    Job = job
                });
            }
        }
    }
}
