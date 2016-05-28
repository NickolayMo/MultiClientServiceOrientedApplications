using CarRental.Business.Common;
using CarRental.Business.Entities;
using CarRental.Common.Exceptions;
using CarRental.Data.Contract.RepositoryIntefaces;
using Core.Common.Contracts;
using Core.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace CarRental.Business.BusinessEngines
{
    [Export(typeof(ICarRentalEngine))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CarRentalEngine:ICarRentalEngine
    {
        IDataRepositoryFactory _DataRepositoryFactory;

        [ImportingConstructor]
        public CarRentalEngine(IDataRepositoryFactory dataRepositoryFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }
        public bool IsCarAvailableForRental(int carId, DateTime pickUpDate, DateTime returnDate, IEnumerable<Rental> rentedCars, IEnumerable<Reservation> reservedCars)
        {
            bool available = true;
            Reservation reservation = reservedCars.Where(item => item.CarId == carId).FirstOrDefault();
            if (reservation != null && 
                (pickUpDate >= reservation.RentalDate && pickUpDate<= reservation.ReturnDate) ||
                (returnDate >= reservation.RentalDate && returnDate <= reservation.ReturnDate))
            {
                available = false;
                
            }
            if (available)
            {
                Rental rental = rentedCars.Where(item => item.CarId == carId).FirstOrDefault();
                if (rental != null && (pickUpDate <= rental.DateDue)) { }
                {
                    available = false;
                }
            }
            return available;
        }

        public bool IsCarCurrentlyRented(int carId)
        {
            bool rented = false;
            IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();

            Rental rental = rentalRepository.GetRentalByCar(carId);
            if (rental != null)
            {
                rented = true; 
            }
            return rented;
        }

        public bool IsCarCurrentlyRented(int carId, int accountId)
        {
            bool rented = false;
            IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();

            Rental rental = rentalRepository.GetRentalByCar(carId);
            if (rental != null && rental.AccountId == accountId)
            {
                rented = true;
            }
            return rented;
        }

        public Rental RentCarToCustomer(string loginEmail, int carId, DateTime rentalDate, DateTime dueDate)
        {
            if (rentalDate > DateTime.Now)
            {
                throw new UnableToRentForDateException($"Can not rent for date {rentalDate} yet");
            }
            IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
            IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();
            bool carIsRented = IsCarCurrentlyRented(carId);
            if (carIsRented)
            {
                throw new CarCurrentlyRentedException($"Car {carId} is alredy rented");
            }
            Account account = accountRepository.GetByLogin(loginEmail);
            if (account == null)
            {
                throw new NotFoundException($"No account for {loginEmail}");
            }
            Rental rental = new Rental()
            {
                AccountId = account.AccountId,
                CarId = carId,
                DateDue = dueDate,
                DateRented = rentalDate
            };
            Rental savedEntity = rentalRepository.Add(rental);
            return savedEntity;
        }
    }
}
