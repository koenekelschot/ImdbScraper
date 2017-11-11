using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using ImdbScraper.Models;

namespace ImdbScraper.Parsers
{
    public class PersonParser : IParser<Person>
    {
        private IHtmlDocument _document;
        private Person _result;

        public async Task<Person> ParseAsync(string html)
        {
            HtmlParser parser = new HtmlParser();
            _result = new Person();
            _document = await parser.ParseAsync(html);

            try
            {
                ParseGenericProperties();
                ParseKnownAs();
                ParseBio();
                ParseBirth();
                ParseDeath();

                return _result;
            }
            catch (HtmlParseException e)
            {
                throw new ParserException("Error parsing HTML for Person.", e);
            }
        }

        private void ParseGenericProperties()
        {
            var idElement = _document.QuerySelector("meta[property='pageId']");
            var nameElement = _document.QuerySelector("h3[itemprop='name'] a[itemprop='url']");
            var posterElement = _document.QuerySelector("img.poster");
            
            _result.Id = idElement.GetAttribute("content");
            _result.Name = nameElement.TextContent.Trim();
            _result.Adult = false;
            _result.Poster = posterElement?.GetAttribute("src");
        }

        private void ParseKnownAs()
        {
            const string birthNameLabelText = "Birth Name";
            const string nicknameLabelText = "Nickname";
            var labelElements = _document.QuerySelectorAll("table#overviewTable td.label");

            foreach (var labelElement in labelElements)
            {
                if (!labelElement.TextContent.Equals(birthNameLabelText, StringComparison.OrdinalIgnoreCase) &&
                    !labelElement.TextContent.Equals(nicknameLabelText, StringComparison.OrdinalIgnoreCase))
                    continue;

                string contents = labelElement.NextElementSibling.TextContent.Trim();
                _result.KnownAs.Add(contents);
            }
        }

        private void ParseBio()
        {
            var bioAnchor = _document.QuerySelector("div#bio_content a[name='mini_bio']");
            var bioContainer = bioAnchor?.NextElementSibling?.NextElementSibling;
            if (bioContainer != null && bioContainer.HasChildNodes)
            {
                _result.Biography = Stringify(bioContainer.Children[0]).Trim();
            }
        }

        private void ParseBirth()
        {
            const string labelText = "Date of Birth";
            const string selector = "table#overviewTable td.label";
            _result.BirthDay = ParseDateElement(selector, labelText);
            _result.BirthPlace = ParseLocationElement(selector, labelText);
        }

        private void ParseDeath()
        {
            const string labelText = "Date of Death";
            const string selector = "table#overviewTable td.label";
            _result.DeathDay = ParseDateElement(selector, labelText);

            string deathDetails = ParseLocationElement(selector, labelText);
            if (string.IsNullOrWhiteSpace(deathDetails))
                return;

            if (deathDetails.IndexOf('(') > -1)
            {
                string[] deathPlaceAndCause = deathDetails.Split('(');
                _result.DeathPlace = deathPlaceAndCause[0].Trim();
                string deathCause = deathPlaceAndCause[1].Trim();
                _result.DeathCause = deathCause.Substring(0, deathCause.Length - 1);
            }
            else
            {
                _result.DeathPlace = deathDetails;
            }
        }

        private DateTime? ParseDateElement(string querySelector, string labelText)
        {
            var labelElements = _document.QuerySelectorAll(querySelector);

            foreach (var labelElement in labelElements)
            {
                if (!labelElement.TextContent.Equals(labelText, StringComparison.OrdinalIgnoreCase))
                    continue;

                var contentElement = labelElement.NextElementSibling;
                string contents = contentElement.TextContent.Trim();
                string[] contentParts = contents.Split(new[] { ',' }, 1);

                if (!contentParts.Any())
                    continue;

                var dateElements = contentElement.QuerySelectorAll("a");
                string monthDay = dateElements[0].TextContent;
                string year = dateElements[1].TextContent;

                return ParseDate($"{monthDay} {year}");
            }

            return null;
        }

        private string ParseLocationElement(string querySelector, string labelText)
        {
            var labelElements = _document.QuerySelectorAll(querySelector);

            foreach (var labelElement in labelElements)
            {
                if (!labelElement.TextContent.Equals(labelText, StringComparison.OrdinalIgnoreCase))
                    continue;

                var contentElement = labelElement.NextElementSibling;
                string contents = contentElement.TextContent.Trim();
                string[] contentParts = contents.Split(new[] { ',' }, 2);

                if (!contentParts.Any() || contentParts.Length < 2)
                    continue;

                return contentParts[1].Trim();
            }

            return null;
        }

        private static DateTime? ParseDate(string date)
        {
            DateTime output;
            string cleanDate = Regex.Replace(date.Trim(), @"\s+", " ");

            if (DateTime.TryParseExact(cleanDate, "d MMMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out output))
            {
                return output;
            }

            return null;
        }

        //https://stackoverflow.com/questions/34513559/preserving-or-restoring-whitespace-in-textcontent#34514873
        private static string Stringify(INode node)
        {
            switch (node.NodeType)
            {
                case NodeType.Text:
                    return node.TextContent;

                case NodeType.Element:
                    if (node.HasChildNodes)
                    {
                        var sb = new StringBuilder();
                        var isElement = false;

                        foreach (var child in node.ChildNodes)
                        {
                            var isPreviousElement = isElement;
                            var content = Stringify(child);
                            isElement = child.NodeType == NodeType.Element;

                            if (!string.IsNullOrEmpty(content) && isElement && isPreviousElement)
                            {
                                sb.Append(string.Empty);
                            }

                            sb.Append(content);
                        }

                        return sb.ToString();
                    }

                    switch (node.NodeName.ToLowerInvariant())
                    {
                        case "br": return "\n";
                        default: return string.Empty;
                    }

                default:
                    return string.Empty;
            }
        }
    }
}
