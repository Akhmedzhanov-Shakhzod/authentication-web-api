using System.Globalization;

namespace AuthenticationWebApi.Helpers.ApplicationException
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException() : base() { }

        public ForbiddenException(string message) : base(message) { }

        public ForbiddenException(string field, string message)
            : base($"{field}: - {message}") { }

        public ForbiddenException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
