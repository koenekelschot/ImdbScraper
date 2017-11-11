using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using ImdbScraper.Models;

namespace ImdbScraper.Parsers
{
    public class TitleParser : IParser<Title>
    {
        private const NumberStyles NumberStyles = System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.AllowDecimalPoint;

        private IHtmlDocument _document;

        public async Task<Title> ParseAsync(string html)
        {
            HtmlParser parser = new HtmlParser();
            _document = await parser.ParseAsync(html);

            try
            {
                if (!IsTvShow() && !IsEpisode())
                {
                    Movie movie = new Movie();
                    ParseGenericProperties(movie);
                    movie = ParseMovieProperties(movie);
                    return movie;
                }
                if (IsTvShow())
                {
                    Show show = new Show();
                    ParseGenericProperties(show);
                    show = ParseShowProperties(show);
                    return show;
                }
                Episode episode = new Episode();
                ParseGenericProperties(episode);
                episode = ParseEpisodeProperties(episode);
                return episode;
            }
            catch (HtmlParseException e)
            {
                throw new ParserException("Error parsing HTML for Title.", e);
            }
        }

        private bool IsTvShow()
        {
            return _document.QuerySelectorAll("a").Any(e => e.ClassList.Contains("np_episode_guide"));
        }

        private bool IsEpisode()
        {
            return
                _document.QuerySelectorAll("div#title-overview-widget div")
                    .Any(e => e.ClassList.Contains("navigation_panel")) && !IsTvShow();
        }

        private void ParseGenericProperties(Title result)
        {
            var idElement = _document.QuerySelector("meta[property='pageId']");
            var nameElement = _document.QuerySelector("h1[itemprop='name']");
            var summaryElement = _document.QuerySelector("div[itemprop='description'] p");
            var posterElement = _document.QuerySelector("div.poster img");
            var genreElements = _document.QuerySelectorAll("div.subtext span[itemprop='genre']");
            var keywordElements = _document.QuerySelectorAll("div[itemprop='keywords'] span[itemprop='keywords']");
            var voteAverageElement = _document.QuerySelector("div.imdbRating span[itemprop='ratingValue']");
            var voteCountElement = _document.QuerySelector("div.imdbRating span[itemprop='ratingCount']");
            var runtimeElement = _document.QuerySelector("div.title-overview time[itemprop='duration']");

            result.Id = idElement.GetAttribute("content");
            result.Name = nameElement.ChildNodes[0].TextContent.Trim();
            result.Overview = summaryElement.ChildNodes[0].TextContent.Trim();
            result.Poster = posterElement?.GetAttribute("src");

            if (voteAverageElement != null)
            {
                result.VoteAverage = decimal.Parse(voteAverageElement.TextContent, NumberStyles);
            }

            if (voteCountElement != null)
            {
                result.VoteCount = int.Parse(voteCountElement.TextContent, NumberStyles);
            }

            if (runtimeElement != null)
            {
                result.Runtime = ParseRuntime(runtimeElement.GetAttribute("datetime"));
            }

            foreach (var element in genreElements)
            {
                result.Genres.Add(new Genre(element.TextContent));
            }

            foreach (var element in keywordElements)
            {
                result.Keywords.Add(new Keyword(element.TextContent));
            }
            
            ParseCast(result);
        }

        private Movie ParseMovieProperties(Movie result)
        {
            if (IsTvShow() || IsEpisode())
                return null;

            var directorElement = _document.QuerySelector("div.title-overview span[itemprop='director'] a");
            var releaseElement = _document.QuerySelector("meta[itemprop='datePublished']");
            
            result.TagLine = ParseTagline();
            result.Director = new Person()
            {
                Id = ImdbHelper.GetPersonIdFromUrl(directorElement.GetAttribute("href")),
                Name = directorElement.TextContent.Trim()
            };

            if (releaseElement != null)
            {
                result.ReleaseDate = DateTime.ParseExact(releaseElement.GetAttribute("content"), "yyyy-MM-dd",
                    CultureInfo.InvariantCulture);
            }

            return result;
        }

        private Show ParseShowProperties(Show result)
        {
            if (!IsTvShow())
                return null;

            var creatorElements = _document.QuerySelectorAll("div.title-overview span[itemprop='creator'] a");
            var airDateElement = _document.QuerySelectorAll("div#title-overview-widget div.titleBar div.subtext a").Last();
            var episodeCountElement =
                _document.QuerySelector("div.title-overview div.button_panel a.np_episode_guide span.bp_sub_heading");
            var seasonCountElement = _document.QuerySelector("div#title-episode-widget a");

            foreach (var creatorElement in creatorElements)
            {
                result.CreatedBy.Add(new Person()
                {
                    Id = ImdbHelper.GetPersonIdFromUrl(creatorElement.GetAttribute("href")),
                    Name = creatorElement.TextContent.Trim()
                });
            }

            DateTime?[] airDates = ParseAirDates(airDateElement.TextContent.Trim());
            result.FirstAirDate = airDates[0];
            result.LastAirDate = airDates[1];
            result.EpisodeCount = int.Parse(episodeCountElement.TextContent.Split(' ')[0]);
            result.SeasonCount = int.Parse(seasonCountElement.TextContent);

            return result;
        }

        private Episode ParseEpisodeProperties(Episode result)
        {
            if (!IsEpisode())
                return null;

            var airDateElement = _document.QuerySelector("div#title-overview-widget div.titleBar meta[itemprop='datePublished']");
            var seasonEpisodeElement = _document.QuerySelector("div#title-overview-widget div.navigation_panel div.bp_heading");

            int seasonNumber = 0;
            int episodeNumber = 0;
            ParseSeasonAndEpisodeNumbers(seasonEpisodeElement.TextContent, out seasonNumber, out episodeNumber);

            result.AirDate = DateTime.ParseExact(airDateElement.GetAttribute("content"), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            result.SeasonNumber = seasonNumber;
            result.EpisodeNumber = episodeNumber;

            return result;
        }

        private void ParseCast(Title result)
        {
            var castElements = _document.QuerySelectorAll("div#titleCast table.cast_list tr");
            foreach (var castElement in castElements.Where(e => e.ClassName == "odd" || e.ClassName == "even"))
            {
                var actorElement = castElement.QuerySelector("td[itemprop='actor'] a");
                var characterElement = castElement.QuerySelector("td.character");

                result.Credits.Cast.Add(new MediaCast
                {
                    Id = ImdbHelper.GetPersonIdFromUrl(actorElement.GetAttribute("href")),
                    Name = actorElement.TextContent.Trim(),
                    Character = ParseCharacterName(characterElement.TextContent)
                });
            }
        }

        private string ParseTagline()
        {
            const string taglineHeaderText = "Taglines:";
            const string seeMoreText = @"See[ ]more\s»";
            var taglineElements = _document.QuerySelectorAll("div#titleStoryLine div.txt-block");

            foreach (var taglineElement in taglineElements)
            {
                var headerElement = taglineElement.Children.First(c => c.TagName == "H4");
                if (!headerElement.TextContent.Equals(taglineHeaderText, StringComparison.OrdinalIgnoreCase))
                    continue;

                string taglineContents = taglineElement.TextContent;
                var headerRegex = new Regex(taglineHeaderText, RegexOptions.IgnoreCase);
                var seeMoreRegex = new Regex(seeMoreText, RegexOptions.IgnoreCase);
                taglineContents = headerRegex.Replace(taglineContents, string.Empty);
                taglineContents = seeMoreRegex.Replace(taglineContents, string.Empty);
                return taglineContents.Trim();
            }
            return null;
        }

        private static int ParseRuntime(string runtimeString)
        {
            runtimeString = runtimeString.Replace("PT", string.Empty).Replace("M", string.Empty);
            return int.Parse(runtimeString);
        }

        private static DateTime?[] ParseAirDates(string airDatesString)
        {
            DateTime?[] airDates = new DateTime?[2];

            string[] parts = airDatesString.Replace(")", string.Empty).Split('(');
            if (parts.Length >= 1)
            {
                string[] years = parts[1].Split('–');
                airDates[0] = DateTime.ParseExact(years[0], "yyyy", CultureInfo.InvariantCulture);
                if (!string.IsNullOrWhiteSpace(years[1]))
                {
                    airDates[1] = DateTime.ParseExact(years[1], "yyyy", CultureInfo.InvariantCulture);
                }
            }
            
            return airDates;
        }

        private static string ParseCharacterName(string name)
        {
            string[] parts = name.Split('/');
            name = parts[0];
            parts = name.Split('(');
            name = parts[0];
            return name.Trim();
        }

        private static void ParseSeasonAndEpisodeNumbers(string numbersString, out int seasonNumber, out int episodeNumber)
        {
            var seasonRegex = new Regex("Season", RegexOptions.IgnoreCase);
            var episodeRegex = new Regex("Episode", RegexOptions.IgnoreCase);
            string[] parts = numbersString.Split('|');
            parts[0] = seasonRegex.Replace(parts[0], string.Empty).Trim();
            parts[1] = episodeRegex.Replace(parts[1], string.Empty).Trim();

            seasonNumber = int.Parse(parts[0]);
            episodeNumber = int.Parse(parts[1]);
        }
    }
}
