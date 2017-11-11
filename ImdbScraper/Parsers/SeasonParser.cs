using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using ImdbScraper.Models;

namespace ImdbScraper.Parsers
{
    public class SeasonParser : IParser<Season>
    {
        private IHtmlDocument _document;
        private Season _result;

        public async Task<Season> ParseAsync(string html)
        {
            HtmlParser parser = new HtmlParser();
            _result = new Season();
            _document = await parser.ParseAsync(html);

            try
            {
                ParseGenericProperties();
                ParseEpisodes();

                return _result;
            }
            catch (HtmlParseException e)
            {
                throw new ParserException("Error parsing HTML for Season.", e);
            }
        }

        private void ParseGenericProperties()
        {
            const string seasonHeaderText = "Season";
            var nameElement = _document.QuerySelector("h3[itemprop='name'] a");
            var airDateElement = _document.QuerySelector("div.eplist div.info[itemprop='episodes'] div.airdate");
            var posterElement = _document.QuerySelector("div#main img[itemprop='image']");
            var seasonNoElement = _document.QuerySelector("div#episodes_content h3#episode_top");

            _result.Name = nameElement.TextContent.Trim();
            _result.Poster = posterElement?.GetAttribute("src");
            _result.AirDate = TryParseEpisodeDate(airDateElement?.TextContent);

            string seasonNumberText = seasonNoElement.TextContent;
            var regex = new Regex(seasonHeaderText, RegexOptions.IgnoreCase);
            seasonNumberText = regex.Replace(seasonNumberText, string.Empty);
            _result.SeasonNumber = int.Parse(seasonNumberText.Trim());
            
        }

        private void ParseEpisodes()
        {
            var episodeElements = _document.QuerySelectorAll("div#episodes_content div.eplist div.list_item");
            foreach (var episodeElement in episodeElements)
            {
                Episode episode = new Episode();

                var nameElement = episodeElement.QuerySelector("div.image a");
                var overviewElement = episodeElement.QuerySelector("div[itemprop='description']");
                var posterElement = episodeElement.QuerySelector("div.image img");
                var airDateElement = episodeElement.QuerySelector("div.airdate");
                var episodeNoElement = episodeElement.QuerySelector("meta[itemprop='episodeNumber']");

                episode.Id = ImdbHelper.GetTitleIdFromUrl(nameElement.GetAttribute("href"));
                episode.Name = nameElement.GetAttribute("title");
                episode.Overview = overviewElement.TextContent.Trim();
                episode.EpisodeNumber = int.Parse(episodeNoElement.GetAttribute("content"));
                episode.Poster = posterElement?.GetAttribute("src");
                episode.AirDate = TryParseEpisodeDate(airDateElement?.TextContent);
                episode.SeasonNumber = _result.SeasonNumber;

                _result.Episodes.Add(episode);
            }
        }

        private static DateTime? TryParseEpisodeDate(string input)
        {
            string[] formats = new[]
            {
                "d MMM yyyy",
                "MMM yyyy",
                "yyyy"
            };
            DateTime output;
            string cleaned = input.Trim().Replace(".", string.Empty);
            if (DateTime.TryParseExact(cleaned, formats, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out output))
            {
                return output;
            }
            return null;
        }
    }
}
