using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using ImdbScraper.Models;

namespace ImdbScraper.Parsers
{
    public class PersonCreditsParser : IParser<PersonCredits>
    {
        private IHtmlDocument _document;
        private PersonCredits _result;

        public async Task<PersonCredits> ParseAsync(string html)
        {
            HtmlParser parser = new HtmlParser();
            _result = new PersonCredits();
            _document = await parser.ParseAsync(html);

            try
            {
                Parse();
                return _result;
            }
            catch (HtmlParseException e)
            {
                throw new ParserException("Error parsing HTML for PersonCredits.", e);
            }
        }

        private void Parse()
        {
            var filmographyElements = _document.QuerySelectorAll("section#filmography ul li");

            foreach (var filmographyElement in filmographyElements)
            {
                PersonCast casting = ParseFilmographyItem(filmographyElement);
                if (_result.Cast.All(c => c.Id != casting.Id))
                {
                    _result.Cast.Add(casting);
                }
            }
        }

        private static PersonCast ParseFilmographyItem(IElement element)
        {
            var posterElement = element.QuerySelector(".filmo-image img");
            var titleElements = element.QuerySelectorAll(".filmo-caption small");

            string poster = posterElement.GetAttribute("src");
            string titleUrl = titleElements[0].QuerySelector("a").GetAttribute("href");
            string title = titleElements[0].TextContent.Trim();

            string character = null;
            DateTime? releaseDate = null;
            if (titleElements.Length == 2)
            {
                releaseDate = ParseReleaseDate(titleElements[1].TextContent);
            }
            else
            {
                character = titleElements[1].TextContent.Trim();
                releaseDate = ParseReleaseDate(titleElements[2].TextContent);
            }

            return new PersonCast
            {
                Id = ImdbHelper.GetTitleIdFromUrl(titleUrl),
                Title = title,
                Poster = poster,
                Character = character,
                ReleaseDate = releaseDate,
                Adult = false
            };
        }

        private static DateTime? ParseReleaseDate(string text)
        {
            DateTime releaseDate;
            text = text.Trim().Trim(new[] { '(', ')' });
            if (text.Contains("-"))
            {
                string[] splitted = text.Split('-');
                text = splitted[0];
            }

            if (DateTime.TryParseExact(text, "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out releaseDate))
            {
                return releaseDate;
            }
            return null;
        }
    }
}
