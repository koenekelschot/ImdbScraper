using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ImdbScraper.Parsers;
using ImdbScraper.Models;

namespace ImdbScraper
{
    public class ImdbService : IImdbService
    {
        private readonly ImdbConfig _settings;
        private readonly HttpClient _httpClient;

        private readonly IParser<Title> _titleParser = new TitleParser();
        private readonly IParser<Season> _seasonParser = new SeasonParser();
        private readonly IParser<Person> _personParser = new PersonParser(); 
        private readonly IParser<MediaCredits> _mediaCreditsParser = new MediaCreditsParser();
        private readonly IParser<PersonCredits> _personCreditsParser = new PersonCreditsParser();
        private readonly IParser<IEnumerable<Movie>> _movieChartParser = new TitleChartParser<Movie>();
        private readonly IParser<IEnumerable<Show>> _showChartParser = new TitleChartParser<Show>();
        private readonly IParser<IEnumerable<Movie>> _newMoviesParser = new NewMoviesParser();
        private readonly IParser<IEnumerable<Title>> _similarTitlesParser = new SimilarTitlesParser();
        private readonly IParser<IEnumerable<SearchResult>> _searchParser = new SearchResultsParser();

        private static ImdbServiceException NoValidPersonIdException(string id)
            => new ImdbServiceException($"Not a valid person id: {id}.");

        private static ImdbServiceException NoValidTitleIdException(string id)
            => new ImdbServiceException($"Not a valid title id: {id}.");

        private static ImdbServiceException NoValidEpisodeNumberException(string id, int season, int episode)
            => new ImdbServiceException($"Not a valid episode: {id} s{season}e{episode}.");

        private static readonly Encoding Encoding = Encoding.UTF8;

        public ImdbService() : this(new ImdbConfig()) { }

        public ImdbService(ImdbConfig settings)
        {
            _settings = settings;

            _httpClient = new HttpClient(new HttpClientHandler
            {
                AllowAutoRedirect = true,
                UseCookies = false,
                AutomaticDecompression = DecompressionMethods.GZip
            });
        }

        public async Task<IEnumerable<SearchResult>> SearchAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<SearchResult>();

            query = ImdbHelper.FormatSearchQuery(query);

            string url = $"{_settings.SearchUrl}/{query[0]}/{query}.json";
            return await ParseUrlAsync(url, _searchParser);
        }

        public async Task<Person> GetPersonAsync(string id)
        {
            if (!IsValidPersonId(id)) 
                throw NoValidPersonIdException(id);

            string url = _settings.PersonUrl + id + "/bio";
            return await ParseUrlAsync(url, _personParser);
        }

        public async Task<PersonCredits> GetPersonCreditsAsync(string id)
        {
            if (!IsValidPersonId(id))
                throw NoValidPersonIdException(id);

            string url = _settings.FilmographyUrl + id;
            return await ParseUrlAsync(url, _personCreditsParser);
        }

        public async Task<Title> GetTitleAsync(string id)
        {
            if (!IsValidTitleId(id))
                throw NoValidTitleIdException(id);

            string url = _settings.TitleUrl + id;
            return await ParseUrlAsync(url, _titleParser);
        }

        public async Task<Season> GetSeasonAsync(string id, int season)
        {
            if (!IsValidTitleId(id))
                throw NoValidTitleIdException(id);

            string url = _settings.TitleUrl + id + "/episodes?season=" + season;
            return await ParseUrlAsync(url, _seasonParser);
        }

        public async Task<Episode> GetEpisodeByShowIdAsync(string id, int season, int episode)
        {
            if (!IsValidTitleId(id))
                throw NoValidTitleIdException(id);

            Episode requestedEpisode = await GetEpisodeFromSeasonAsync(id, season, episode);
            return (Episode) await GetTitleAsync(requestedEpisode.Id);
        }

        public async Task<Episode> GetEpisodeByIdAsync(string id)
        {
            if (!IsValidTitleId(id))
                throw NoValidTitleIdException(id);

            return (Episode) await GetTitleAsync(id);
        }

        public async Task<MediaCredits> GetTitleCreditsAsync(string id)
        {
            if (!IsValidTitleId(id))
                throw NoValidTitleIdException(id);

            string url = _settings.TitleUrl + id + "/fullcredits";
            return await ParseUrlAsync(url, _mediaCreditsParser);
        }

        public async Task<MediaCredits> GetEpisodeCreditsAsync(string id, int season, int episode)
        {
            if (!IsValidTitleId(id))
                throw NoValidTitleIdException(id);

            Episode requestedEpisode = await GetEpisodeFromSeasonAsync(id, season, episode);
            return await GetTitleCreditsAsync(requestedEpisode.Id);
        }

        public async Task<IEnumerable<Title>> GetSimilarTitlesAsync(string id)
        {
            if (!IsValidTitleId(id))
                throw NoValidTitleIdException(id);

            string url = _settings.TitleUrl + id;
            return await ParseUrlAsync(url, _similarTitlesParser);
        }

        public async Task<IEnumerable<Movie>> GetPopularMoviesAsync()
        {
            string url = _settings.ChartUrl + "/moviemeter";
            return await ParseUrlAsync(url, _movieChartParser);
        }

        public async Task<IEnumerable<Show>> GetPopularShowsAsync()
        {
            string url = _settings.ChartUrl + "/tvmeter";
            return await ParseUrlAsync(url, _showChartParser);
        }

        public async Task<IEnumerable<Movie>> GetTopRatedMoviesAsync()
        {
            string url = _settings.ChartUrl + "/top";
            return await ParseUrlAsync(url, _movieChartParser);
        }

        public async Task<IEnumerable<Show>> GetTopRatedShowsAsync()
        {
            string url = _settings.ChartUrl + "/toptv";
            return await ParseUrlAsync(url, _showChartParser);
        }

        public async Task<IEnumerable<Movie>> GetNowPlayingMoviesAsync()
        {
            string url = _settings.InTheatersUrl;
            return await ParseUrlAsync(url, _newMoviesParser);
        }

        public async Task<IEnumerable<Movie>> GetUpcomingMoviesAsync()
        {
            string url = _settings.ComingSoonUrl;
            return await ParseUrlAsync(url, _newMoviesParser);
        }

        public async Task<string> GetIdOfTitleAsync(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return null;

            string url = CreateSafeTitleIdSearchUrl(title);
            var response = await _httpClient.GetAsync(url);
            string html = await HandleResponseAsync(response);
            IEnumerable<string> ids = GetIdsFromLinks(html);

            return ids?.First();
        }

        private static bool IsValidTitleId(string id)
        {
            return ImdbHelper.IsValidTitleId(id);
        }

        private static bool IsValidPersonId(string id)
        {
            return ImdbHelper.IsValidPersonId(id);
        }

        private static bool IsValidCharacterId(string id)
        {
            return ImdbHelper.IsValidCharacterId(id);
        }

        private async Task<T> ParseUrlAsync<T>(string url, IParser<T> parser) where T : class
        {
            var response = await _httpClient.GetAsync(url);
            string html = await HandleResponseAsync(response);
            return await parser.ParseAsync(html);
        }

        private static async Task<string> HandleResponseAsync(HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new ImdbServiceException($"HTTP {response.StatusCode}: {response.ReasonPhrase}");
            }

            return await response.Content.ReadAsStringAsync();
        }

        private async Task<Episode> GetEpisodeFromSeasonAsync(string showId, int season, int episode)
        {
            Season requestedSeason = await GetSeasonAsync(showId, season);

            if (requestedSeason.Episodes.All(e => e.EpisodeNumber != episode))
                throw NoValidEpisodeNumberException(showId, season, episode);

            return requestedSeason.Episodes.First(e => e.EpisodeNumber == episode);
        }

        private string CreateSafeTitleIdSearchUrl(string param)
        {
            return _settings.TitleIdSearchUrl + "?q=" + WebUtility.UrlEncode(param);
        }

        private static IEnumerable<string> GetIdsFromLinks(string html)
        {
            Regex regex = new Regex("<a[^>]*? href=\"(?<url>[^\"]+)\"[^>]*?>(?<text>.*?)</a>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            var matches = regex.Matches(html);
            foreach (Match m in matches)
            {
                string url = m.Groups["url"].Value;
                string id = ImdbHelper.GetTitleIdFromUrl(url);
                if (ImdbHelper.IsValidTitleId(id))
                {
                    yield return id;
                }
            }
        }
    }
}
