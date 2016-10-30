using CarRental.Client.Contracts.ServiceContracts;
using Core.Common.ServiceModel;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace CarRental.Client.Proxies.Proxies
{
    [Export(typeof(IInventoryService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class InventoryClient : UserClientBase<IInventoryService>, IInventoryService
    {
        public void DeleteCar(int carId)
        {
            Channel.DeleteCar(carId);
        }

        public Task DeleteCarAsync(int carId)
        {
            return Channel.DeleteCarAsync(carId);
        }

        public Entities.Car[] GetAllAvailableCars(DateTime pickUpDate, DateTime returnDate)
        {
            
             return Channel.GetAllAvailableCars(pickUpDate, returnDate);
        }

        public Task<Entities.Car[]> GetAllAvailableCarsAsync(DateTime pickUpDate, DateTime returnDate)
        {
            return Channel.GetAllAvailableCarsAsync(pickUpDate, returnDate);
        }

        public Entities.Car[] GetAllCars()
        {
            return Channel.GetAllCars();
        }

        public Task<Entities.Car[]> GetAllCarsAsync()
        {
            return Channel.GetAllCarsAsync();
        }

        public Entities.Car GetCar(int carId)
        {
            return Channel.GetCar(carId);
        }

        public Task<Entities.Car> GetCarAsync(int carId)
        {
            return Channel.GetCarAsync(carId);
        }

        public Entities.Car UpdateCar(Entities.Car car)
        {
            return Channel.UpdateCar(car);
        }

        public Task<Entities.Car> UpdateCarAsync(Entities.Car car)
        {
            return Channel.UpdateCarAsync(car);
        }
    }
}
