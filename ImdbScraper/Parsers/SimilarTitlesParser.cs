using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using ImdbScraper.Models;

namespace ImdbScraper.Parsers
{
    public class SimilarTitlesParser : IParser<IEnumerable<Title>>
    {
        private const string ShowIndicatorText = "TV Series";
        
        private IHtmlDocument _document;
        private IList<Title> _result;
        
        public async Task<IEnumerable<Title>> ParseAsync(string html)
        {
            HtmlParser parser = new HtmlParser();
            _result = new List<Title>();
            _document = await parser.ParseAsync(html);

            try
            {
                Parse();
                return _result;
            }
            catch (HtmlParseException e)
            {
                throw new ParserException("Error parsing HTML for SimilarTitles.", e);
            }
        }

        private void Parse()
        {
            var titleElements = _document.QuerySelectorAll("div#title_recs div.rec_overview");
            foreach (IElement element in titleElements)
            {
                _result.Add(ParseTitleElement(element));
            }
        }

        private static Title ParseTitleElement(IElement titleElement)
        {
            var nameElement = titleElement.QuerySelector("div.rec-title a");
            var posterElement = titleElement.QuerySelector("div.rec_poster img.rec_poster_img");
            var genreElement = titleElement.QuerySelector("div.rec-cert-genre");
            var overviewElement = titleElement.QuerySelector("div.rec-outline");
            var dateElement = titleElement.QuerySelector("div.rec-title span.nobr");

            Title title = new Title();

            title.Id = ImdbHelper.GetTitleIdFromUrl(nameElement.GetAttribute("href"));
            title.Name = nameElement.TextContent.Trim();
            title.Overview = overviewElement.TextContent.Trim();
            title.Genres = ParseGenres(genreElement);
            title.ReleaseDate = ParseDate(dateElement.TextContent);

            if (posterElement != null)
            {
                title.Poster = posterElement.GetAttribute("src");
            }

            return title;
        }
        
        private static DateTime? ParseDate(string date)
        {
            DateTime releaseDate;
            date = date.Trim().Trim(new[] { '(', ')' });
            date = date.Replace(ShowIndicatorText, string.Empty).Trim();

            if (DateTime.TryParseExact(date, "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out releaseDate))
            {
                return releaseDate;
            }
            return null;
        }
        
        private static IList<Genre> ParseGenres(IElement genresElement)
        {
            string content = genresElement.TextContent.Trim();
            string[] genres = content.Split('|');

            return genres.Select(genre => new Genre(genre.Trim())).ToList();
        }
    }
}
