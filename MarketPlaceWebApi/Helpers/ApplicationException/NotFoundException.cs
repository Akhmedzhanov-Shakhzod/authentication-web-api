using System.Globalization;

namespace MarketPlaceWebApi.Helpers.ApplicationException
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base() { }

        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string field, string message)
            : base($"{field}: - {message}") { }

        public NotFoundException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
