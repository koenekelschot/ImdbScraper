using System.Collections.Generic;

namespace ImdbScraper.Models
{
    public class MediaCredits
    {
        public IList<MediaCast> Cast { get; set; }

        public IList<MediaCrew> Crew { get; set; }

        public MediaCredits()
        {
            Cast = new List<MediaCast>();
            Crew = new List<MediaCrew>();
        }
    }

    public abstract class MediaCredit
    {
        /// <summary>
        /// Id of the Imdb Name (actor)
        /// </summary>
        public string Id { get; set; }

        //public string CreditId { get; set; }

        public string Name { get; set; }

        //public string Profile { get; set; }
    }

    public class MediaCast : MediaCredit
    {
        public string Character { get; set; }
    }

    public class MediaCrew : MediaCredit
    {
        public string Department { get; set; }

        public string Job { get; set; }
    }
}
