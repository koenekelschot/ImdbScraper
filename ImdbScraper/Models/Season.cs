using System;
using System.Collections.Generic;

namespace ImdbScraper.Models
{
    public class Season
    {
        public string Name { get; set; }

        //public string Overview { get; set; }

        public DateTime? AirDate { get; set; }

        public string Poster { get; set; }

        public int SeasonNumber { get; set; }

        //public MediaCredits Credits { get; set; }

        //public IEnumerable<Video> Videos { get; set; }

        public IList<Episode> Episodes { get; set; }

        public Season()
        {
            //Videos = Enumerable.Empty<Video>();
            Episodes = new List<Episode>();
        }
    }
}
