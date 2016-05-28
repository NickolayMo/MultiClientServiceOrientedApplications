using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Common.Core;
using CarRental.Client.Bootstrapper;
using CarRental.Client.Contracts.ServiceContracts;
using CarRental.Client.Proxies.Proxies;
using Core.Common.Contracts;

namespace CarRental.Client.Proxies.Test
{
    [TestClass]
    public class ProxiesObtaimentTests
    {
        [TestInitialize]
        public void Initialize()
        {
            ObjectBase.Container = MEFLoader.Init();
        }
        [TestMethod]
        public void obtain_proxy_from_container_using_service_contract()
        {
            IInventoryService proxy = ObjectBase.Container.GetExportedValue<IInventoryService>();
            Assert.IsTrue(proxy is InventoryClient);
        }
        [TestMethod]
        public void obtain_proxy_from_factory()
        {
            IServiceFactory factory = new ServiceFactory();
            IInventoryService proxy = factory.CreateClient<IInventoryService>();
            Assert.IsTrue(proxy is InventoryClient);
        }

        [TestMethod]
        public void obtain_proxy_from_container()
        {
            IServiceFactory factory = ObjectBase.Container.GetExportedValue<IServiceFactory>();
            IInventoryService proxy = factory.CreateClient<IInventoryService>();
            Assert.IsTrue(proxy is InventoryClient);
        }
    }
}
