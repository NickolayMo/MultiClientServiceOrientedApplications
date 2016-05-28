using System;

namespace CarRental.Common.Exceptions
{
    public class CarNotRentedException : ApplicationException
    {
        public CarNotRentedException(string message) : base(message)
        {
        }

        public CarNotRentedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
