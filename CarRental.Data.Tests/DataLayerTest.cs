using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Common.Core;
using CarRental.Business.Bootstrapper;
using System.ComponentModel.Composition;
using CarRental.Data.Contract.RepositoryIntefaces;
using System.Collections.Generic;
using CarRental.Business.Entities;
using Moq;
using Core.Common.Contracts;

namespace CarRental.Data.Tests
{
    [TestClass]
    public class DataLayerTest
    {
        [TestInitialize]
        public void Intializer()
        {
            ObjectBase.Container = MEFLoader.Init();
        }
        [TestMethod]
        public void test_repository_usage()
        {
            RepoositoryTestClass repository = new RepoositoryTestClass();
            IEnumerable<Car> cars = repository.GetCars();
            Assert.IsNotNull(cars);
        }

        [TestMethod]
        public void test_repository_moking()
        {
            List<Car> cars = new List<Car>()
            {
                new Car { CarId=1, Description="Mustang"},
                new Car {CarId = 2, Description="Vaz" }
            };
            Mock<ICarRepository> mock = new Mock<ICarRepository>();
            mock.Setup(obj => obj.Get()).Returns(cars);
            RepoositoryTestClass repository = new RepoositoryTestClass(mock.Object);
            IEnumerable<Car> testCars = repository.GetCars();
            Assert.IsTrue(testCars == cars);
        }
        [TestMethod]
        public void test_repository_factory_usage()
        {
            RepositoryFactoryTestClass factory = new RepositoryFactoryTestClass();
            IEnumerable<Car> cars = factory.GetCars();
            Assert.IsNotNull(cars);
        }

        [TestMethod]
        public void test_repository_factory_mocking()
        {

            List<Car> cars = new List<Car>()
            {
                new Car {CarId = 1, Description="Mustang" },
                new Car {CarId = 2, Description="Vaz" }
            };
            Mock<IDataRepositoryFactory> mock = new Mock<IDataRepositoryFactory>();
            mock.Setup(obj => obj.GetDataRepository<ICarRepository>().Get()).Returns(cars);
            RepositoryFactoryTestClass factory = new RepositoryFactoryTestClass(mock.Object);
            IEnumerable<Car> dcars = factory.GetCars();
            Assert.IsTrue(cars == dcars);
        }
        [TestMethod]
        public void test_repository_factory_mocking2()
        {
            List<Car> cars = new List<Car>()
            {
                new Car {CarId = 1, Description="Mustang" },
                new Car {CarId = 2, Description="Vaz" }
            };
            Mock<ICarRepository> mockCar = new Mock<ICarRepository>();
            mockCar.Setup(obj => obj.Get()).Returns(cars);
            Mock<IDataRepositoryFactory> mock = new Mock<IDataRepositoryFactory>();           
            mock.Setup(obj => obj.GetDataRepository<ICarRepository>()).Returns(mockCar.Object);

            RepositoryFactoryTestClass factory = new RepositoryFactoryTestClass(mock.Object);
            IEnumerable<Car> dcars = factory.GetCars();
            Assert.IsTrue(cars == dcars);

        }

    }

    public class RepoositoryTestClass
    {
        public RepoositoryTestClass(ICarRepository repository)
        {
            _CarRepository = repository;
        }
        public RepoositoryTestClass()
        {
            ObjectBase.Container.SatisfyImportsOnce(this);

        }
        [Import]
        private ICarRepository _CarRepository;

        public IEnumerable<Car> GetCars()
        {
            IEnumerable<Car> cars = _CarRepository.Get();
            return cars;
        }
    }

    public class RepositoryFactoryTestClass
    {
        public RepositoryFactoryTestClass()
        {
            ObjectBase.Container.SatisfyImportsOnce(this);
        }
        public RepositoryFactoryTestClass(IDataRepositoryFactory factory)
        {
            _factory = factory;

        }

        [Import]
        private IDataRepositoryFactory _factory;

        public IEnumerable<Car> GetCars()
        {
            ICarRepository carRep = _factory.GetDataRepository<ICarRepository>();
            IEnumerable<Car> cars = carRep.Get();
            return cars;
        }
    }
}
