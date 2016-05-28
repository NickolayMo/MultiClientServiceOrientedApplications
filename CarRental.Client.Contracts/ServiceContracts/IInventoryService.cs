using CarRental.Client.Entities;
using Core.Common.Contracts;
using Core.Common.Exceptions;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CarRental.Client.Contracts.ServiceContracts
{
    [ServiceContract]
    public interface IInventoryService: IServiceContract
    {
        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        Car GetCar(int carId);

        [OperationContract]
        Car[] GetAllCars();

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        Car UpdateCar(Car car);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void DeleteCar(int carId);

        [OperationContract]
        Car[] GetAllAvailableCars(DateTime pickUpDate, DateTime returnDate);

        [OperationContract]      
        Task<Car> GetCarAsync(int carId);

        [OperationContract]
        Task<Car[]> GetAllCarsAsync();

        [OperationContract]
        Task<Car> UpdateCarAsync(Car car);

        [OperationContract]
        Task DeleteCarAsync(int carId);

        [OperationContract]
        Task<Car[]> GetAllAvailableCarsAsync(DateTime pickUpDate, DateTime returnDate);
    }
}
