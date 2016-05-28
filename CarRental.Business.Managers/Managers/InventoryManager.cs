using CarRental.Business.Contracts.ServiceContracts;
using Core.Common.Contracts;
using Core.Common.Core;
using System.ComponentModel.Composition;
using CarRental.Business.Entities;
using System;
using CarRental.Data.Contract.RepositoryIntefaces;
using Core.Common.Exceptions;
using System.ServiceModel;
using System.Collections.Generic;
using System.Linq;
using CarRental.Business.Common;
using System.Security.Permissions;
using CarRental.Common;

namespace CarRental.Business.Managers.Managers
{
    [ServiceBehavior(
        ConcurrencyMode = ConcurrencyMode.Multiple,
        InstanceContextMode = InstanceContextMode.PerCall, 
        ReleaseServiceInstanceOnTransactionComplete = false
        )]
    public class InventoryManager:ManagerBase, IInventoryService
    {
        [Import]
        IDataRepositoryFactory _DataRepositoryFactory;

        [Import]
        IBusinessEngineFactory _BusinessEngineFactory;

        public InventoryManager()
        {
            ObjectBase.Container.SatisfyImportsOnce(this);
        }
        public InventoryManager (IDataRepositoryFactory dataReppositoryFactory)
        {
            _DataRepositoryFactory = dataReppositoryFactory;
        }
        public InventoryManager(IBusinessEngineFactory factory)
        {
            _BusinessEngineFactory = factory;
        }

        public InventoryManager(IBusinessEngineFactory businessFactory, IDataRepositoryFactory dataRepositoryFactory)
        {
            _BusinessEngineFactory = businessFactory;
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CAR_RENTAL_ADMIN)]
        public void DeleteCar(int carId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                ICarRepository carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository>();

                carRepository.Remove(carId);
            });
        }
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CAR_RENTAL_ADMIN)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CAR_RENTAL_USER)]
        public Car[] GetAllAvailableCars(DateTime pickUpDate, DateTime returnDate)
        {
            return ExecuteFaultHandledOperation(()=> {
                ICarRepository carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository>();
                IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();
                IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();
                
                ICarRentalEngine engine = _BusinessEngineFactory.GetBusinessEngine<ICarRentalEngine>();

                IEnumerable<Car> allCars = carRepository.Get();
                IEnumerable<Rental> rentedCars = rentalRepository.GetCurrentlyRentedCars();
                IEnumerable<Reservation> reservedCars = reservationRepository.Get();
                List<Car> availablesCars = new List<Car>();

                foreach (var car in allCars)
                {
                    if (engine.IsCarAvailableForRental(car.CarId, pickUpDate,returnDate,rentedCars,reservedCars))
                    {
                        availablesCars.Add(car);
                    }
                }

                return new List<Car>().ToArray();
            });
        }
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CAR_RENTAL_ADMIN)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CAR_RENTAL_USER)]
        public Car[] GetAllCars()
        {
            return ExecuteFaultHandledOperation(()=> {
                ICarRepository carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository>();
                IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();
                IEnumerable<Car> cars = carRepository.Get();
                IEnumerable<Rental> rentedCars = rentalRepository.GetCurrentlyRentedCars();
                foreach (var car in cars)
                {
                    Rental rentedCar = rentedCars.Where(item => item.CarId == car.CarId).FirstOrDefault();
                    car.CurrentlyRented = (rentedCar != null);
                }
                return cars.ToArray();
            });
           
        }
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CAR_RENTAL_ADMIN)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CAR_RENTAL_USER)]
        public Car GetCar(int carId)
        {
            return ExecuteFaultHandledOperation(()=> {

                ICarRepository carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository>();
                Car carEntity = carRepository.Get(carId);
                if (carEntity == null)
                {
                    NotFoundException ex = new NotFoundException($"Car whis id:{carId} not found in database");
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }
                return carEntity;

            });
            
        }
        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CAR_RENTAL_ADMIN)]
        public Car UpdateCar(Car car)
        {
            return ExecuteFaultHandledOperation(()=> {
                ICarRepository carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository>();
                Car updatedEntity = null;
                if (car.CarId == 0)
                {
                    updatedEntity = carRepository.Add(car);
                }
                else
                {
                    updatedEntity = carRepository.Update(car);
                }
                return updatedEntity;

            });
        }
    }
}
