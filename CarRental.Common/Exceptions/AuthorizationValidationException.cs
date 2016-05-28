using System;

namespace CarRental.Common.Exceptions
{
    public class AuthorizationValidationException : ApplicationException
    {
        public AuthorizationValidationException(string message) : base(message)
        {
        }

        public AuthorizationValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
