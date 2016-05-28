using CarRental.Client.DataContracts;
using CarRental.Client.Entities;
using CarRental.Common.Exceptions;
using Core.Common.Contracts;
using Core.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CarRental.Client.Contracts.ServiceContracts
{
    [ServiceContract]
    public interface IRentalService : IServiceContract
    {
        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        IEnumerable<Rental> GetCarRentalHistory(string loginEmail);

        [OperationContract(Name = "RentCarToCustomerImmediately")]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        [FaultContract(typeof(CarCurrentlyRentedException))]
        [FaultContract(typeof(UnableToRentForDateException))]
        Rental RentCarToCastomer(string loginEmail, int carId, DateTime dueDate);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        [FaultContract(typeof(CarCurrentlyRentedException))]
        [FaultContract(typeof(UnableToRentForDateException))]
        Rental RentCarToCastomer(string loginEmail, int carId, DateTime rentalDate, DateTime dueDate);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        [FaultContract(typeof(CarNotRentedException))]
        void AcceptCarReturn(int carId);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Reservation GetReservation(int reservationId);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Reservation MakeReservation(string loginEmail, int carId, DateTime rentalDate, DateTime returnDate);


        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        [FaultContract(typeof(CarCurrentlyRentedException))]
        [FaultContract(typeof(UnableToRentForDateException))]
        void ExecuteRentalFromReservation(int reservationId);


        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        void CancelReservation(int reservationId);


        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        CustomerReservationData[] GetCurrentReservations();

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        CustomerReservationData[] GetCustomerReservations(string loginEmail);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Rental GetRental(int rentalId);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        CustomerRentalData[] GetCurrentRentals();

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Reservation[] GetDeadReservations();

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        bool IsCarCurrentlyRented(int carId);

        [OperationContract]
        Task<IEnumerable<Rental>> GetCarRentalHistoryAsync(string loginEmail);

        [OperationContract(Name = "RentCarToCustomerImmediately")]
        Task<Rental> RentCarToCastomerAsync(string loginEmail, int carId, DateTime dueDate);

        [OperationContract]
        Task<Rental> RentCarToCastomerAsync(string loginEmail, int carId, DateTime rentalDate, DateTime dueDate);

        [OperationContract]       
        Task AcceptCarReturnAsync(int carId);

        [OperationContract]      
        Task<Reservation> GetReservationAsync(int reservationId);

        [OperationContract]
        Task<Reservation> MakeReservationAsync(string loginEmail, int carId, DateTime rentalDate, DateTime returnDate);


        [OperationContract]
        Task ExecuteRentalFromReservationAsync(int reservationId);


        [OperationContract]
        Task CancelReservationAsync(int reservationId);


        [OperationContract]
        Task<CustomerReservationData[]> GetCurrentReservationsAsync();

        [OperationContract]
        Task<CustomerReservationData[]> GetCustomerReservationsAsync(string loginEmail);

        [OperationContract]
        Task<Rental> GetRentalAsync(int rentalId);

        [OperationContract]
        Task<CustomerRentalData[]> GetCurrentRentalsAsync();

        [OperationContract]
        Task<Reservation[]> GetDeadReservationsAsync();

        [OperationContract]
        Task<bool> IsCarCurrentlyRentedAsync(int carId);
    }
}
