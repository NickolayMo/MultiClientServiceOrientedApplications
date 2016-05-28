using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarRental.Business.Entities;
using Moq;
using Core.Common.Contracts;
using CarRental.Data.Contract.RepositoryIntefaces;
using CarRental.Business.Managers.Managers;
using System.Security.Principal;
using CarRental.Common;
using System.Threading;

namespace CarRental.Business.Manages.Tests
{
    [TestClass]
    public class InventoryManagerTests
    {
        [TestInitialize]
        public void Initializer()
        {
            GenericPrincipal principal = new GenericPrincipal(
                new GenericIdentity("Kolya"), new string[] { Security.CAR_RENTAL_ADMIN });
            Thread.CurrentPrincipal = principal;
        }
        [TestMethod]
        public void UpdateCar_add_new()
        {
            Car newCar = new Car();
            Car addedCar = new Car
            {
                CarId = 1
            };
            Mock<IDataRepositoryFactory> factory = new Mock<IDataRepositoryFactory>();
            factory.Setup(obj => obj.GetDataRepository<ICarRepository>().Add(newCar)).Returns(addedCar);

            InventoryManager manager = new InventoryManager(factory.Object);
            Car addedEntity = manager.UpdateCar(newCar);
            Assert.IsTrue(addedCar == addedEntity);

        }

        [TestMethod]
        public void UpdateCar_update_existing()
        {
            Car car = new Car()
            {
                CarId = 1
            };
            Car updatedCar = new Car
            {
                CarId = 2
            };
            Mock<IDataRepositoryFactory> factory = new Mock<IDataRepositoryFactory>();
            factory.Setup(obj => obj.GetDataRepository<ICarRepository>().Update(car)).Returns(updatedCar);

            InventoryManager manager = new InventoryManager(factory.Object);
            Car updatedEntity = manager.UpdateCar(car);
            Assert.IsTrue(updatedCar == updatedEntity);
        }
    }
}
