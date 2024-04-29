﻿using System.Globalization;

namespace AuthenticationWebApi.Helpers.ApplicationException
{
    public class BadRequestException : Exception
    {
        public BadRequestException() : base() { }

        public BadRequestException(string message) : base(message) { }

        public BadRequestException(string field, string message)
            : base($"{field}: - {message}") { }

        public BadRequestException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
