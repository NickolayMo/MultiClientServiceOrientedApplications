﻿using CarRental.Business.Entities;
using Core.Common.Exceptions;
using System;
using System.ServiceModel;

namespace CarRental.Business.Contracts.ServiceContracts
{
    [ServiceContract]
    public interface IInventoryService
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
    }
}
