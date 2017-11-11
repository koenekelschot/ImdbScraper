using System;
using System.Collections.Generic;

namespace ImdbScraper.Models
{
    public class Person
    {
        public string Id { get; set; }

        public string Name { get; set; }

        //todo: where to retrieve this?
        public bool Adult { get; set; }

        public IList<string> KnownAs { get; set; }

        public string Biography { get; set; }

        public DateTime? BirthDay { get; set; }

        public string BirthPlace { get; set; }

        public DateTime? DeathDay { get; set; }

        public string DeathPlace { get; set; }

        public string DeathCause { get; set; }

        //public string HomePage { get; set; }

        public string Poster { get; set; }

        public PersonCredits Credits { get; set; }

        public Person()
        {
            KnownAs = new List<string>();
            Credits = new PersonCredits();
        }
    }
}
