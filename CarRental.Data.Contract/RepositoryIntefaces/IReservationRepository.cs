using CarRental.Business.Entities;
using CarRental.Data.Contract.DTOs;
using Core.Common.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CarRental.Data.Contract.RepositoryIntefaces
{
    public interface IReservationRepository:IDataRepository<Reservation>
    {
        IEnumerable<Reservation> GetReservationByPickUpDate(DateTime pickUpDate);
        IEnumerable<CustomerReservationInfo> GetCurrentCustomerReservationInfo();
        IEnumerable<CustomerReservationInfo> GetCustomerOpenReservationInfo(int accountId);
    }
}
