using CarRental.Client.Contracts.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRental.Client.DataContracts;
using CarRental.Client.Entities;
using System.ComponentModel.Composition;
using Core.Common.ServiceModel;

namespace CarRental.Client.Proxies.Proxies

{
    [Export(typeof(IRentalService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class RentalClient : UserClientBase<IRentalService>, IRentalService
    {
        public void AcceptCarReturn(int carId)
        {
            Channel.AcceptCarReturn(carId);
        }

        public Task AcceptCarReturnAsync(int carId)
        {
            return Channel.AcceptCarReturnAsync(carId);
        }

        public void CancelReservation(int reservationId)
        {
            Channel.CancelReservation(reservationId);
        }

        public Task CancelReservationAsync(int reservationId)
        {
            return Channel.CancelReservationAsync(reservationId);
        }

        public void ExecuteRentalFromReservation(int reservationId)
        {
            Channel.ExecuteRentalFromReservation(reservationId);
        }

        public Task ExecuteRentalFromReservationAsync(int reservationId)
        {
            return Channel.ExecuteRentalFromReservationAsync(reservationId);
        }

        public IEnumerable<Rental> GetCarRentalHistory(string loginEmail)
        {
            return Channel.GetCarRentalHistory(loginEmail);
        }

        public Task<IEnumerable<Rental>> GetCarRentalHistoryAsync(string loginEmail)
        {
            return Channel.GetCarRentalHistoryAsync(loginEmail);
        }

        public CustomerRentalData[] GetCurrentRentals()
        {
            return Channel.GetCurrentRentals();
        }

        public Task<CustomerRentalData[]> GetCurrentRentalsAsync()
        {
            return Channel.GetCurrentRentalsAsync();
        }

        public CustomerReservationData[] GetCurrentReservations()
        {
            return Channel.GetCurrentReservations();
        }

        public Task<CustomerReservationData[]> GetCurrentReservationsAsync()
        {
            return Channel.GetCurrentReservationsAsync();
        }

        public CustomerReservationData[] GetCustomerReservations(string loginEmail)
        {
            return Channel.GetCustomerReservations(loginEmail);
        }

        public Task<CustomerReservationData[]> GetCustomerReservationsAsync(string loginEmail)
        {
            return Channel.GetCustomerReservationsAsync(loginEmail);
        }

        public Reservation[] GetDeadReservations()
        {
            return Channel.GetDeadReservations();
        }

        public Task<Reservation[]> GetDeadReservationsAsync()
        {
            return Channel.GetDeadReservationsAsync();
        }

        public Rental GetRental(int rentalId)
        {
            return Channel.GetRental(rentalId);
        }

        public Task<Rental> GetRentalAsync(int rentalId)
        {
            return Channel.GetRentalAsync(rentalId);
        }

        public Reservation GetReservation(int reservationId)
        {
            return Channel.GetReservation(reservationId);
        }

        public Task<Reservation> GetReservationAsync(int reservationId)
        {
            return Channel.GetReservationAsync(reservationId);
        }

        public bool IsCarCurrentlyRented(int carId)
        {
            return Channel.IsCarCurrentlyRented(carId);
        }

        public Task<bool> IsCarCurrentlyRentedAsync(int carId)
        {
            return Channel.IsCarCurrentlyRentedAsync(carId);
        }

        public Reservation MakeReservation(string loginEmail, int carId, DateTime rentalDate, DateTime returnDate)
        {
            return Channel.MakeReservation(loginEmail, carId, rentalDate, returnDate);
        }

        public Task<Reservation> MakeReservationAsync(string loginEmail, int carId, DateTime rentalDate, DateTime returnDate)
        {
            return Channel.MakeReservationAsync(loginEmail, carId, rentalDate, returnDate);
        }

        public Rental RentCarToCastomer(string loginEmail, int carId, DateTime dueDate)
        {
            return Channel.RentCarToCastomer(loginEmail, carId, dueDate);
        }

        public Rental RentCarToCastomer(string loginEmail, int carId, DateTime rentalDate, DateTime dueDate)
        {
            return RentCarToCastomer(loginEmail, carId, rentalDate, dueDate);
        }

        public Task<Rental> RentCarToCastomerAsync(string loginEmail, int carId, DateTime dueDate)
        {
            return Channel.RentCarToCastomerAsync(loginEmail, carId, dueDate);
        }

        public Task<Rental> RentCarToCastomerAsync(string loginEmail, int carId, DateTime rentalDate, DateTime dueDate)
        {
            return Channel.RentCarToCastomerAsync(loginEmail, carId, rentalDate, dueDate);
        }
    }
}
