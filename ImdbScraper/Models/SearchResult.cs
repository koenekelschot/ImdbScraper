namespace ImdbScraper.Models
{
    public class SearchResult
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Image { get; set; }
        public string Type { get; set; }
        public string Details { get; set; }
        public int? Year { get; set; }
    }
}
