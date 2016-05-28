using CarRental.Business.Entities;
using CarRental.Data.Contract.DTOs;
using Core.Common.Contracts;
using System.Collections.Generic;

namespace CarRental.Data.Contract.RepositoryIntefaces
{
    public interface IRentalRepository:IDataRepository<Rental>
    {
        IEnumerable<Rental> GetRentalHistoryByCar(int carId);
        Rental GetRentalByCar(int carId);
        IEnumerable<Rental> GetCurrentlyRentedCars();
        IEnumerable<Rental> GetRentalHistoryByAccount(int accountId);
        IEnumerable<CustomerRentalInfo> GetCurrentCustomerRentalInfo();
    }
}
