using System;

namespace ImdbScraper
{
    public class ImdbServiceException : Exception
    {
        public ImdbServiceException(string message) : this(message, null) { }

        public ImdbServiceException(string message, Exception innerException) : base(message, innerException) { }
    }
}
