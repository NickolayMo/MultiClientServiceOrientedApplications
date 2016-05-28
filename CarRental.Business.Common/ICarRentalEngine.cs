using CarRental.Business.Entities;
using Core.Common.Contracts;
using System;
using System.Collections.Generic;

namespace CarRental.Business.Common
{
    public interface ICarRentalEngine:IBusinessEngine
    {
        bool IsCarAvailableForRental(int carId, DateTime pickUpDate, DateTime returnDate, IEnumerable<Rental> rentedCars, IEnumerable<Reservation> reservedCars);
        Rental RentCarToCustomer(string loginEmail, int carId, DateTime rentalDate, DateTime dueDate);
        bool IsCarCurrentlyRented(int carId);
        bool IsCarCurrentlyRented(int carId, int accountId);
    }
}
