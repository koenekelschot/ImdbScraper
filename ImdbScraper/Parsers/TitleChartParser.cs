using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using ImdbScraper.Models;

namespace ImdbScraper.Parsers
{
    public class TitleChartParser<T> : IParser<IEnumerable<T>> where T : Title, new()
    {
        private const NumberStyles NumberStyles = System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.AllowDecimalPoint;

        private IHtmlDocument _document;
        private IList<T> _result;
        
        public async Task<IEnumerable<T>> ParseAsync(string html)
        {
            HtmlParser parser = new HtmlParser();
            _result = new List<T>();
            _document = await parser.ParseAsync(html);

            try
            {
                Parse();
                return _result;
            }
            catch (HtmlParseException e)
            {
                throw new ParserException("Error parsing HTML for TitleChart.", e);
            }
        }

        private void Parse()
        {
            var rows = _document.QuerySelectorAll("div#main table.chart tbody tr");
            foreach (IElement row in rows)
            {
                _result.Add(ParseRow(row));
            }
        }

        private static T ParseRow(IElement row)
        {
            T entry = new T();

            var posterElement = row.QuerySelector("td.posterColumn img");
            var titleElement = row.QuerySelector("td.titleColumn a");
            var dateElement = row.QuerySelector("td.titleColumn span.secondaryInfo");
            var ratingElement = row.QuerySelector("td.imdbRating strong");

            entry.Id = ImdbHelper.GetTitleIdFromUrl(titleElement.GetAttribute("href"));
            entry.Name = titleElement.TextContent.Trim();
            entry.Poster = posterElement?.GetAttribute("src");
            entry.ReleaseDate = ParseDate(dateElement.TextContent.Trim());

            if (ratingElement != null)
            {
                entry.VoteAverage = decimal.Parse(ratingElement.TextContent, NumberStyles);
            }

            return entry;
        }
        
        private static DateTime? ParseDate(string date)
        {
            DateTime releaseDate;
            date = date.Trim().Trim(new[] {'(', ')'});
            
            if (DateTime.TryParseExact(date, "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out releaseDate))
            {
                return releaseDate;
            }
            return null;
        }
    }
}
