using System;

namespace CarRental.Common.Exceptions
{
    public class UnableToRentForDateException : ApplicationException
    {
        public UnableToRentForDateException(string message) : base(message)
        {
        }

        public UnableToRentForDateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}