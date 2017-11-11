using System.Collections.Generic;
using System.Threading.Tasks;
using ImdbScraper.Models;

namespace ImdbScraper
{
    public interface IImdbService
    {
        /// <summary>
        /// Search for movies, shows, and people
        /// </summary>
        /// <param name="query">Search query.</param>
        Task<IEnumerable<SearchResult>> SearchAsync(string query);

        /// <summary>
		/// Get the general person information for a specific id.
		/// </summary>
		/// <param name="id">The IMDB id of the person.</param>
		/// <exception cref="ImdbServiceException"></exception>
		Task<Person> GetPersonAsync(string id);

        /// <summary>
        /// Get the movie credits for a specific person id.
        /// </summary>
        /// <param name="id">The IMDB id of the person.</param>
        /// <exception cref="ImdbServiceException"></exception>
        Task<PersonCredits> GetPersonCreditsAsync(string id);

        /// <summary>
        /// Get the basic information for a specific title id.
        /// </summary>
        /// <param name="id">The IMDB id of the title.</param>
        /// <exception cref="ImdbServiceException"></exception>
        Task<Title> GetTitleAsync(string id);

        /// <summary>
		/// Get the primary information about a TV season by its season number.
		/// </summary>
		/// <param name="id">The IMDB id of the title.</param>
		/// <param name="season">The season number.</param>
        /// <exception cref="ImdbServiceException"></exception>
		Task<Season> GetSeasonAsync(string id, int season);

        /// <summary>
        /// Get the primary information about a TV episode by combination of a season and episode number.
        /// </summary>
        /// <param name="id">The IMDB id of the show.</param>
		/// <param name="season">The season number.</param>
		/// <param name="episode">The episode number.</param>
        /// <exception cref="ImdbServiceException"></exception>
        Task<Episode> GetEpisodeByShowIdAsync(string id, int season, int episode);

        /// <summary>
        /// Get the primary information about a TV episode by its id.
        /// </summary>
        /// <param name="id">The IMDB id of the episode.</param>
        /// <exception cref="ImdbServiceException"></exception>
        Task<Episode> GetEpisodeByIdAsync(string id);

        /// <summary>
        /// Get the cast and crew information for a specific title id.
        /// </summary>
        /// <param name="id">The IMDB id of the title.</param>
        /// <exception cref="ImdbServiceException"></exception>
        Task<MediaCredits> GetTitleCreditsAsync(string id);

        /// <summary>
        /// Get the cast and crew information for a TV episode by combination of a season and episode number.
        /// </summary>
        /// <param name="id">The IMDB id of the title.</param>
		/// <param name="season">The season number.</param>
		/// <param name="episode">The episode number.</param>
        /// <exception cref="ImdbServiceException"></exception>
        Task<MediaCredits> GetEpisodeCreditsAsync(string id, int season, int episode);

        /// <summary>
        /// Get the similar movies for a specific title id.
        /// </summary>
        /// <param name="id">The IMDB id of the title.</param>
        /// <exception cref="ImdbServiceException"></exception>
        Task<IEnumerable<Title>> GetSimilarTitlesAsync(string id);

        /// <summary>
		/// Get the list of popular movies. This list refreshes every day.
		/// </summary>
		Task<IEnumerable<Movie>> GetPopularMoviesAsync();

        /// <summary>
		/// Get the list of popular TV shows. This list refreshes every day.
		/// </summary>
		Task<IEnumerable<Show>> GetPopularShowsAsync();

        /// <summary>
        /// Get the list of top rated movies. This list refreshes every day.
        /// </summary>
        Task<IEnumerable<Movie>> GetTopRatedMoviesAsync();

        /// <summary>
        /// Get the list of top rated TV shows. This list refreshes every day.
        /// </summary>
        Task<IEnumerable<Show>> GetTopRatedShowsAsync();

        /// <summary>
        /// Get the list of movies playing in theatres. This list refreshes every day. 
        /// The maximum number of items this list will include is 100.
        /// </summary>
        Task<IEnumerable<Movie>> GetNowPlayingMoviesAsync();

        /// <summary>
        /// Get the list of upcoming movies. This list refreshes every day. 
        /// The maximum number of items this list will include is 100.
        /// </summary>
        Task<IEnumerable<Movie>> GetUpcomingMoviesAsync();

        /// <summary>
        /// Get the IMDb id of movies and TV shows
        /// </summary>
        /// <param name="title">The title to get the id for.</param>
        Task<string> GetIdOfTitleAsync(string title);
    }
}
