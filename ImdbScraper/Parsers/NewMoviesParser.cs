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
    public class NewMoviesParser : IParser<IEnumerable<Movie>>
    {
        private IHtmlDocument _document;
        private IList<Movie> _result;
        
        public async Task<IEnumerable<Movie>> ParseAsync(string html)
        {
            HtmlParser parser = new HtmlParser();
            _result = new List<Movie>();
            _document = await parser.ParseAsync(html);

            try
            {
                Parse();
                return _result;
            }
            catch (HtmlParseException e)
            {
                throw new ParserException("Error parsing HTML for NewMovies.", e);
            }
        }

        private void Parse()
        {
            var movieElements = _document.QuerySelectorAll("div#main div.list.detail div.list_item");
            foreach (IElement element in movieElements)
            {
                _result.Add(ParseMovieElement(element));
            }
        }

        private static Movie ParseMovieElement(IElement movieElement)
        {
            var titleElement = movieElement.QuerySelector("h4[itemprop='name'] a");
            var posterElement = movieElement.QuerySelector("td#img_primary div.image img.poster");
            var detailsElement = movieElement.QuerySelector("p.cert-runtime-genre");
            var runtimeElement = detailsElement.QuerySelector("time[itemprop='duration']");
            var genreElements = detailsElement.QuerySelectorAll("span[itemprop='genre']");
            var overviewElement = movieElement.QuerySelector("div.outline[itemprop='description']");
            var directorElement = movieElement.QuerySelector("span[itemprop='director'] a");
            var actorElements = movieElement.QuerySelectorAll("span[itemprop='actors'] a");

            string[] titleParts = titleElement.TextContent.Split('(');

            Movie entry = new Movie();

            entry.Id = ImdbHelper.GetTitleIdFromUrl(titleElement.GetAttribute("href"));
            entry.Name = titleParts[0].Trim();
            entry.ReleaseDate = ParseDate(titleParts[1]);
            entry.Poster = posterElement?.GetAttribute("src");
            entry.Overview = overviewElement?.TextContent?.Trim();
            entry.Genres = ParseGenres(genreElements);
            entry.Director = ParseDirector(directorElement);
            entry.Credits.Cast = ParseCast(actorElements);

            if (runtimeElement != null)
            {
                entry.Runtime = ParseRuntime(runtimeElement.GetAttribute("datetime"));
            }
            
            return entry;
        }

        private static DateTime? ParseDate(string date)
        {
            DateTime releaseDate;
            date = date.Trim().Trim(new[] { '(', ')' });

            if (DateTime.TryParseExact(date, "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out releaseDate))
            {
                return releaseDate;
            }
            return null;
        }

        private static IList<Genre> ParseGenres(IHtmlCollection<IElement> genreElements)
        {
            return genreElements.Select(genreElement => new Genre(genreElement.TextContent.Trim())).ToList();
        }

        private static int ParseRuntime(string runtimeString)
        {
            runtimeString = runtimeString.Replace("PT", string.Empty).Replace("M", string.Empty);
            return int.Parse(runtimeString);
        }

        private static Person ParseDirector(IElement directorElement)
        {
            Person director = new Person
            {
                Id = ImdbHelper.GetPersonIdFromUrl(directorElement.GetAttribute("href")),
                Name = directorElement.TextContent.Trim()
            };

            return director;
        }

        private static IList<MediaCast> ParseCast(IHtmlCollection<IElement> actorElements)
        {
            return actorElements.Select(actorElement => new MediaCast
            {
                Id = ImdbHelper.GetPersonIdFromUrl(actorElement.GetAttribute("href")),
                Name = actorElement.TextContent.Trim()
            }).ToList();
        }
    }
}
