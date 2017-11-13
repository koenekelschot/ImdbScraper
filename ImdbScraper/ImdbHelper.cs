using System;
using System.Linq;
using System.Text.RegularExpressions;
using Unidecode.NET;

namespace ImdbScraper
{
    public static class ImdbHelper
    {
        private static Regex TitleIdRegex => new Regex(@"tt[0-9]{7}", RegexOptions.IgnoreCase);
        private static Regex PersonIdRegex => new Regex(@"nm[0-9]{7}", RegexOptions.IgnoreCase);
        private static Regex CharacterIdRegex => new Regex(@"ch[0-9]{7}", RegexOptions.IgnoreCase);

        public static bool IsValidTitleId(string id)
        {
            return !string.IsNullOrEmpty(id) && TitleIdRegex.IsMatch(id);
        }

        public static bool IsValidPersonId(string id)
        {
            return !string.IsNullOrEmpty(id) && PersonIdRegex.IsMatch(id);
        }

        public static bool IsValidCharacterId(string id)
        {
            return !string.IsNullOrEmpty(id) && CharacterIdRegex.IsMatch(id);
        }

        public static string GetIdFromUrl(string url)
        {
            string id = GetTitleIdFromUrl(url);
            if (!string.IsNullOrEmpty(id))
            {
                return id;
            }
            id = GetPersonIdFromUrl(url);
            if (!string.IsNullOrEmpty(id))
            {
                return id;
            }
            id = GetCharacterIdFromUrl(url);
            if (!string.IsNullOrEmpty(id))
            {
                return id;
            }
            return null;
        }

        public static string GetTitleIdFromUrl(string url)
        {
            string[] parts = GetUrlParts(url);
            return parts.FirstOrDefault(part => IsValidTitleId(part));
        }

        public static string GetPersonIdFromUrl(string url)
        {
            string[] parts = GetUrlParts(url);
            return parts.FirstOrDefault(part => IsValidPersonId(part));
        }

        public static string GetCharacterIdFromUrl(string url)
        {
            string[] parts = GetUrlParts(url);
            return parts.FirstOrDefault(part => IsValidCharacterId(part));
        }

        public static string FormatSearchQuery(string query)
        {
            query = query.Trim();
            query = query.Unidecode();
            query = Regex.Replace(query, "[^A-Za-z0-9 ]", string.Empty);
            query = Regex.Replace(query, @"\s", "_");
            query = query.ToLower();
            return query;
        }

        private static string[] GetUrlParts(string url)
        {
            var queryStringIndex = url.IndexOf("?", StringComparison.OrdinalIgnoreCase);
            if (queryStringIndex > -1)
            {
                url = url.Substring(0, queryStringIndex);
            }
            return url.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
