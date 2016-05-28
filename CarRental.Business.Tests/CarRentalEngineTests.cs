using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarRental.Business.Entities;
using Moq;
using CarRental.Data.Contract.RepositoryIntefaces;
using Core.Common.Contracts;
using CarRental.Business.BusinessEngines;

namespace CarRental.Business.Tests
{
    [TestClass]
    public class CarRentalEngineTests
    {
        [TestMethod]
        public void IsCarCurrentlyRented_any_account()
        {
            Rental rental = new Rental()
            {
                CarId = 1
            };
            Mock<IRentalRepository> mockRentalRepository = new Mock<IRentalRepository>();
            mockRentalRepository.Setup(obj => obj.GetRentalByCar(1)).Returns(rental);

            Mock<IDataRepositoryFactory> mockFactory = new Mock<IDataRepositoryFactory>();
            mockFactory.Setup(obj => obj.GetDataRepository<IRentalRepository>()).Returns(mockRentalRepository.Object);

            CarRentalEngine engine = new CarRentalEngine(mockFactory.Object);

            Assert.IsTrue(engine.IsCarCurrentlyRented(1));
            Assert.IsFalse(engine.IsCarCurrentlyRented(2));
        }
    }
}
