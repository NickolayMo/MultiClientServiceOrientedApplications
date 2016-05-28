using System;

namespace CarRental.Common.Exceptions
{
    public class CarCurrentlyRentedException : ApplicationException
    {
        public CarCurrentlyRentedException(string message) : base(message)
        {
        }

        public CarCurrentlyRentedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
