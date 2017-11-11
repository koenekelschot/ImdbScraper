namespace ImdbScraper
{
    public class ImdbConfig
    {
        public string SearchUrl { get; set; } = "http://sg.media-imdb.com/suggests/";
        public string TitleUrl { get; set; } = "http://www.imdb.com/title/";
        public string PersonUrl { get; set; } = "http://www.imdb.com/name/";
        public string FilmographyUrl { get; set; } = "http://m.imdb.com/name/";
        public string ChartUrl { get; set; } = "http://www.imdb.com/chart/";
        public string InTheatersUrl { get; set; } = "http://www.imdb.com/movies-in-theaters/";
        public string ComingSoonUrl { get; set; } = "http://www.imdb.com/movies-coming-soon/";
        public string TitleIdSearchUrl { get; set; } = "https://duckduckgo.com/";
    }
}
