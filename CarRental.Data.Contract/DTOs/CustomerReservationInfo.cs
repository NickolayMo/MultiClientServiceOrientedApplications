using CarRental.Business.Entities;

namespace CarRental.Data.Contract.DTOs
{
    public class CustomerReservationInfo
    {
        public Account Customer { get; set; }
        public Car Car { get; set; }
        public Reservation Reservation { get; set; }
    }
}
