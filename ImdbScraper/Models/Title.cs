using System;
using System.Collections.Generic;
using System.Linq;

namespace ImdbScraper.Models
{
    public class Title
    {
        public string Id { get; set; }

        public string Name { get; set; }

        //public string OriginalName { get; set; }

        public string Overview { get; set; }

        public string Poster { get; set; }

        public IList<Genre> Genres { get; set; }

        //public string HomePage { get; set; }

        public MediaCredits Credits { get; set; }

        //public IEnumerable<Video> Videos { get; set; }

        public IList<Keyword> Keywords { get; set; }

        //public decimal Popularity { get; set; }

        public decimal? VoteAverage { get; set; }

        public int? VoteCount { get; set; }

        //public string Status { get; set; }

        public int? Runtime { get; set; }

        public virtual DateTime? ReleaseDate { get; set; }

        public Title()
        {
            Genres = new List<Genre>();
            //Videos = Enumerable.Empty<Video>();
            Keywords = new List<Keyword>();
            Credits = new MediaCredits();
        }
    }

    public class Show : Title
    {
        //public IList<Network> Networks { get; set; }

        public IList<Person> CreatedBy { get; set; }

        /// <summary>
        /// Year only
        /// </summary>
        public DateTime? FirstAirDate { get; set; }

        /// <summary>
        /// Year only
        /// </summary>
        public DateTime? LastAirDate { get; set; }

        public override DateTime? ReleaseDate
        {
            get { return FirstAirDate; }
            set { FirstAirDate = value; }
        }
        
        //public bool InProduction { get; set; }

        public int EpisodeCount { get; set; }

        public int SeasonCount { get; set; }

        //public IList<int> EpisodeRuntimes { get; set; }

        public IList<Season> Seasons { get; set; }

        public Show()
        {
            //Networks = new List<Network>();
            CreatedBy = new List<Person>();
            //EpisodeRuntimes = new List<int>();
            Seasons = new List<Season>();
        }
    }

    public class Movie : Title
    {
        public string TagLine { get; set; }

        public bool Adult => Genres.Any(g => g.Name.Equals("adult", StringComparison.OrdinalIgnoreCase));

        public Person Director { get; set; }
        
        //public int Budget { get; set; }

        //public long Revenue { get; set; }
    }

    public class Episode : Title
    {
        public DateTime? AirDate { get; set; }

        public override DateTime? ReleaseDate
        {
            get { return AirDate; }
            set { AirDate = value; }
        }

        public bool Adult => Genres.Any(g => g.Name.Equals("adult", StringComparison.OrdinalIgnoreCase));

        public int SeasonNumber { get; set; }

        public int EpisodeNumber { get; set; }
    }
}
