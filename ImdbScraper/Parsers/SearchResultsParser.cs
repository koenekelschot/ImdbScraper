using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImdbScraper.Models;
using Newtonsoft.Json;

namespace ImdbScraper.Parsers
{
    public class SearchResultsParser : IParser<IEnumerable<SearchResult>>
    {
        private string _json;
        private IEnumerable<SearchResult> _result;
        
        public Task<IEnumerable<SearchResult>> ParseAsync(string json)
        {
            _json = json;
            _result = new List<SearchResult>();

            try
            {
                Parse();
                return Task.FromResult(_result);
            }
            catch (Exception e)
            {
                throw new ParserException("Error parsing HTML for SearchResults.", e);
            }
        }

        private void Parse()
        {
            int startIndex = _json.IndexOf("{", StringComparison.Ordinal);
            int endIndex = _json.LastIndexOf("}", StringComparison.Ordinal);

            if (startIndex == -1 || endIndex == -1 || endIndex <= startIndex)
                return;

            string json = _json.Substring(0, endIndex + 1).Substring(startIndex);
            _result = ParseSearchResults(json);
        }

        private static IEnumerable<SearchResult> ParseSearchResults(string json)
        {
            dynamic jsonResponse = JsonConvert.DeserializeObject(json);
            foreach (var result in jsonResponse.d)
            {
                yield return ParseSingleResult(result);
            }
        }

        private static SearchResult ParseSingleResult(dynamic result)
        {
            return new SearchResult
            {
                Id = result.id,
                Name = result.l,
                Type = result.q ?? "person",
                Details = result.s,
                Year = result.y,
                Image = result.i[0]
            };
        }
    }
}
