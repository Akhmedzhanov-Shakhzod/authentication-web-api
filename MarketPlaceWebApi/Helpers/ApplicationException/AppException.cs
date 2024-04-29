using System.Globalization;

namespace MarketPlaceWebApi.Helpers.ApplicationException
{
    public class AppException : Exception
    {
        public AppException() : base() { }

        public AppException(string message) : base(message) { }

        public AppException(string field, string message)
            : base($"{field}: - {message}") { }

        public AppException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
