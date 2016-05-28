using CarRental.Business.Contracts.ServiceContracts;
using System;
using System.Collections.Generic;
using CarRental.Business.Entities;
using System.ServiceModel;
using Core.Common.Contracts;
using Core.Common.Core;
using System.ComponentModel.Composition;
using CarRental.Data.Contract.RepositoryIntefaces;
using Core.Common.Exceptions;
using System.Security.Permissions;
using CarRental.Common;
using CarRental.Business.DataContracts;
using CarRental.Business.Common;
using CarRental.Common.Exceptions;
using CarRental.Data.Contract.DTOs;
using System.Linq;

namespace CarRental.Business.Managers.Managers
{
    [ServiceBehavior(ConcurrencyMode=ConcurrencyMode.Multiple, 
        InstanceContextMode = InstanceContextMode.PerCall,
        ReleaseServiceInstanceOnTransactionComplete = false)]
    public class RentalManager : ManagerBase, IRentalService
    {
        [Import]
        IDataRepositoryFactory _DataRepositoryFactory;

        [Import]
        IBusinessEngineFactory _BusinessEngineFactory;

        public RentalManager()
        {
            ObjectBase.Container.SatisfyImportsOnce(this);
        }
        public RentalManager (IDataRepositoryFactory dataReppositoryFactory)
        {
            _DataRepositoryFactory = dataReppositoryFactory;
        }
        public RentalManager(IBusinessEngineFactory factory)
        {
            _BusinessEngineFactory = factory;
        }

        public RentalManager(IBusinessEngineFactory businessFactory, IDataRepositoryFactory dataRepositoryFactory)
        {
            _BusinessEngineFactory = businessFactory;
            _DataRepositoryFactory = dataRepositoryFactory;
        }
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CAR_RENTAL_ADMIN)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CAR_RENTAL_USER)]
        public IEnumerable<Rental> GetCarRentalHistory(string loginEmail)
        {
            return ExecuteFaultHandledOperation(()=>{
                IAccountRepository accountRep = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
                IRentalRepository rental = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();
                Account account = accountRep.GetByLogin(loginEmail);
                if (account == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format($"No account found for login {loginEmail}"));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }
                ValidateAutorization(account);
                IEnumerable<Rental> rentalHistory = rental.GetRentalHistoryByAccount(account.AccountId);
                return rentalHistory;
            });
        }
        protected override Account LoadAutarizationValidationAccount(string loginName)
        {
            IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
            Account account = accountRepository.GetByLogin(loginName);
            if (account == null)
            {
                NotFoundException ex = new NotFoundException($"Cannot find accoun for login name to use for security trimming");
                throw new FaultException<NotFoundException>(ex, ex.Message);
            }
            return account;

        }
        [OperationBehavior(TransactionScopeRequired =true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CAR_RENTAL_ADMIN)]
        public Rental RentCarToCastomer(string loginEmail, int carId, DateTime rentalDate, DateTime dueDate)
        {
            return ExecuteFaultHandledOperation(()=> {
                ICarRentalEngine carRentalEngine = _BusinessEngineFactory.GetBusinessEngine<ICarRentalEngine>();
                try
                {
                    Rental rental = carRentalEngine.RentCarToCustomer(loginEmail, carId, rentalDate, dueDate);
                    return rental;
                }
                catch (UnableToRentForDateException ex)
                {

                    throw new FaultException<UnableToRentForDateException>(ex, ex.Message);
                }
                catch(CarCurrentlyRentedException ex)
                {
                    throw new FaultException<CarCurrentlyRentedException>(ex, ex.Message);
                }
                catch(NotFoundException ex)
                {
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }
                
            });
        }
        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CAR_RENTAL_ADMIN)]
        public void AcceptCarReturn(int carId)
        {
            ExecuteFaultHandledOperation(()=> {
                IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();
                ICarRentalEngine carRentalEngine = _BusinessEngineFactory.GetBusinessEngine<ICarRentalEngine>();
                Rental rental = rentalRepository.GetRentalByCar(carId);
                if (rental==null)
                {
                    CarNotRentedException ex = new CarNotRentedException($"Car id {carId} is not currently rented");
                    throw new FaultException<CarNotRentedException>(ex, ex.Message);
                }
                rental.DateReturned = DateTime.Now;
                Rental updatedRentalEntity = rentalRepository.Update(rental);

            });
        }
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CAR_RENTAL_ADMIN)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CAR_RENTAL_USER)]
        public Reservation GetReservation(int reservationId)
        {
            return ExecuteFaultHandledOperation(()=> {
                IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();
                Reservation reservation = reservationRepository.Get(reservationId);
                if (reservation == null)
                {
                    NotFoundException ex = new NotFoundException($"No reservation found for id {reservationId}");
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }
                ValidateAutorization(reservation);
                return reservation;
            });
        }
        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CAR_RENTAL_ADMIN)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CAR_RENTAL_USER)]
        public Reservation MakeReservation(string loginEmail, int carId, DateTime rentalDate, DateTime returnDate)
        {
            return ExecuteFaultHandledOperation(()=> {
                IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
                IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();

                Account account = accountRepository.GetByLogin(loginEmail);
                if (account == null)
                {
                    NotFoundException ex = new NotFoundException($"No account found by login {loginEmail}");
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }
                ValidateAutorization(account);
                Reservation reservation = new Reservation()
                {
                    AccountId = account.AccountId,
                    CarId = carId,
                    RentalDate = rentalDate,
                    ReturnDate = returnDate
                };
                Reservation savedEntity = reservationRepository.Add(reservation);
                return savedEntity;

            });
        }
        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CAR_RENTAL_ADMIN)]
        public void ExecuteRentalFromReservation(int reservationId)
        {
            ExecuteFaultHandledOperation(()=> {
                IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
                IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();
                ICarRentalEngine carRentalEngine = _BusinessEngineFactory.GetBusinessEngine<ICarRentalEngine>();
                Reservation reservation = reservationRepository.Get(reservationId);
                if (reservation == null)
                {
                    NotFoundException ex = new NotFoundException($"Reservation {reservationId} not found");
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }
                Account account = accountRepository.Get(reservation.AccountId);
                if (account == null)
                {
                    NotFoundException ex = new NotFoundException($"Account {reservationId} not found");
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }
                try
                {
                    Rental rental = carRentalEngine.RentCarToCustomer(account.LoginEmail, reservation.CarId, reservation.RentalDate, reservation.ReturnDate);
                }
                catch (UnableToRentForDateException ex)
                {
                    throw new FaultException<UnableToRentForDateException>(ex, ex.Message);
                }
                catch (CarCurrentlyRentedException ex)
                {
                    throw new FaultException<CarCurrentlyRentedException>(ex, ex.Message);
                }
                catch (NotFoundException ex)
                {
                    throw new FaultException<NotFoundException>(ex, ex.Message);

                }
                reservationRepository.Remove(reservation);
            });          

        }
        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role =Security.CAR_RENTAL_ADMIN)]
        [PrincipalPermission(SecurityAction.Demand, Name =Security.CAR_RENTAL_USER)]
        public void CancelReservation(int reservationId)
        {
            ExecuteFaultHandledOperation(()=> {
                IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();
                Reservation reservation = reservationRepository.Get(reservationId);
                if (reservation==null)
                {
                    NotFoundException ex = new NotFoundException($"No reservation found by id {reservationId}");
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }
                ValidateAutorization(reservation);
                reservationRepository.Remove(reservationId);
            });
        }
        
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CAR_RENTAL_ADMIN)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CAR_RENTAL_USER)]
        public CustomerReservationData[] GetCurrentReservations()
        {
            return ExecuteFaultHandledOperation(()=> {
                IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();
                List<CustomerReservationData> reservationData = new List<CustomerReservationData>();
                IEnumerable<CustomerReservationInfo> reservationInfoSet = reservationRepository.GetCurrentCustomerReservationInfo();
                foreach (var reservaionInfo in reservationInfoSet)
                {
                    reservationData.Add(
                        new CustomerReservationData()
                        {
                            ReservationId = reservaionInfo.Reservation.ReservationId,
                            Car = reservaionInfo.Car.Color + " " + reservaionInfo.Car.Year + " " + reservaionInfo.Car.Description,
                            CustomerName = reservaionInfo.Customer.FirstName+" "+reservaionInfo.Customer.LastName,
                            DateRented = reservaionInfo.Reservation.RentalDate,
                            ReturnDate = reservaionInfo.Reservation.ReturnDate
                        });
                }
                return reservationData.ToArray();
            });
        }

        
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CAR_RENTAL_ADMIN)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CAR_RENTAL_USER)]
        public CustomerReservationData[] GetCustomerReservations(string loginEmail)
        {
            return ExecuteFaultHandledOperation(() => {
                IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();
                IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
                Account account = accountRepository.GetByLogin(loginEmail);
                if (account == null)
                {
                    NotFoundException ex = new NotFoundException($"Account {loginEmail} not found");
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }
                ValidateAutorization(account);
                List<CustomerReservationData> reservationData = new List<CustomerReservationData>();
                IEnumerable<CustomerReservationInfo> reservationInfoSet = reservationRepository.GetCustomerOpenReservationInfo(account.AccountId);
                foreach (var reservaionInfo in reservationInfoSet)
                {
                    reservationData.Add(
                        new CustomerReservationData()
                        {
                            ReservationId = reservaionInfo.Reservation.ReservationId,
                            Car = reservaionInfo.Car.Color + " " + reservaionInfo.Car.Year + " " + reservaionInfo.Car.Description,
                            CustomerName = reservaionInfo.Customer.FirstName + " " + reservaionInfo.Customer.LastName,
                            DateRented = reservaionInfo.Reservation.RentalDate,
                            ReturnDate = reservaionInfo.Reservation.ReturnDate
                        });
                }
                return reservationData.ToArray();
            });
        }
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CAR_RENTAL_ADMIN)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CAR_RENTAL_USER)]
        public Rental GetRental(int rentalId)
        {
            IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();
            Rental rental = rentalRepository.Get(rentalId);
            if (rental == null)
            {
                NotFoundException ex = new NotFoundException($"Rental {rentalId} not found");
                throw new FaultException<NotFoundException>(ex, ex.Message);
            }
            ValidateAutorization(rental);
            return rental;
        }
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CAR_RENTAL_ADMIN)]
        public CustomerRentalData[] GetCurrentRentals()
        {
            return ExecuteFaultHandledOperation(() => {
                IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();
                List<CustomerRentalData> rentalData = new List<CustomerRentalData>();
                IEnumerable<CustomerRentalInfo> rentalInfoSet = rentalRepository.GetCurrentCustomerRentalInfo();
                foreach (var rentalInfo in rentalInfoSet)
                {
                    rentalData.Add(
                        new CustomerRentalData()
                        {
                            CarRentalId = rentalInfo.Rental.RentalId,
                            Car = rentalInfo.Car.Color + " " + rentalInfo.Car.Year + " " + rentalInfo.Car.Description,
                            CustomerName = rentalInfo.Customer.FirstName + " " + rentalInfo.Customer.LastName,
                            DateRented = rentalInfo.Rental.DateRented,
                            ExpectedReturn = rentalInfo.Rental.DateReturned
                        });
                }
                return rentalData.ToArray();
            });
        }
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CAR_RENTAL_ADMIN)]
        public Reservation[] GetDeadReservations()
        {
            return ExecuteFaultHandledOperation(() => {
                IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();
                IEnumerable<Reservation> reservations = reservationRepository.GetReservationByPickUpDate(DateTime.Now.AddDays(-1));
                return (reservations != null ? reservations.ToArray() : null);
            });

        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CAR_RENTAL_ADMIN)]
        public bool IsCarCurrentlyRented(int carId)
        {
            return ExecuteFaultHandledOperation(()=> {
                ICarRentalEngine carRentalEngine = _BusinessEngineFactory.GetBusinessEngine<ICarRentalEngine>();
                return carRentalEngine.IsCarCurrentlyRented(carId);
            });
        }
        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CAR_RENTAL_ADMIN)]
        public Rental RentCarToCastomer(string loginEmail, int carId, DateTime dueDate)
        {
            return ExecuteFaultHandledOperation(() => {
                ICarRentalEngine carRentalEngine = _BusinessEngineFactory.GetBusinessEngine<ICarRentalEngine>();
                try
                {
                    Rental rental = carRentalEngine.RentCarToCustomer(loginEmail, carId, DateTime.Now, dueDate);
                    return rental;
                }
                catch (UnableToRentForDateException ex)
                {

                    throw new FaultException<UnableToRentForDateException>(ex, ex.Message);
                }
                catch (CarCurrentlyRentedException ex)
                {
                    throw new FaultException<CarCurrentlyRentedException>(ex, ex.Message);
                }
                catch (NotFoundException ex)
                {
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

            });
        }
       
    }
}
