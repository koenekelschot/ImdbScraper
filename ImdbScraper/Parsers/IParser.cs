using System.Threading.Tasks;

namespace ImdbScraper.Parsers
{
    public interface IParser<T> where T : class
    {
        /// <summary>
        /// Parse the data to an instance of T.
        /// </summary>
        /// <param name="data">String containing the data to parse.</param>
        /// <returns>An instance of T.</returns>
        Task<T> ParseAsync(string data);
    }
}
