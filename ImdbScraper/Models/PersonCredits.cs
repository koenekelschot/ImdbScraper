using System;
using System.Collections.Generic;

namespace ImdbScraper.Models
{
    public class PersonCredits
    {
        public IList<PersonCast> Cast { get; set; }

        public IList<PersonCrew> Crew { get; set; }

        public PersonCredits()
        {
            Cast = new List<PersonCast>();
            Crew = new List<PersonCrew>();
        }
    }

    public abstract class PersonCredit
    {
        public string Id { get; set; }

        //public string CreditId { get; set; }

        public string Title { get; set; }

        //public string OriginalTitle { get; set; }

        public string Poster { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public bool Adult { get; set; }
    }

    public class PersonCast : PersonCredit
    {
        public string Character { get; set; }
    }

    public class PersonCrew : PersonCredit
    {
        public string Department { get; set; }

        public string Job { get; set; }
    }
}
