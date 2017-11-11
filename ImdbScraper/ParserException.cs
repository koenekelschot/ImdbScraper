using System;

namespace ImdbScraper
{
    public class ParserException : Exception
    {
        public ParserException(string message) : this(message, null) { }

        public ParserException(string message, Exception innerException) : base(message, innerException) { }
    }
}
