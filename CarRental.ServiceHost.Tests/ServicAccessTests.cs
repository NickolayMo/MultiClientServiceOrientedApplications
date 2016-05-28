using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;
using CarRental.Business.Contracts.ServiceContracts;

namespace CarRental.ServiceHost.Tests
{
    [TestClass]
    public class ServicAccessTests
    {
        [TestMethod]
        public void test_inventory_manger_as_service()
        {
            ChannelFactory<IInventoryService> channelFactory = new ChannelFactory<IInventoryService>("");
            IInventoryService proxy = channelFactory.CreateChannel();
            (proxy as ICommunicationObject).Open();
            channelFactory.Close();
        }

        [TestMethod]
        public void test_rental_manger_as_service()
        {
            ChannelFactory<IRentalService> channelFactory = new ChannelFactory<IRentalService>("");
            IRentalService proxy = channelFactory.CreateChannel();
            (proxy as ICommunicationObject).Open();
            channelFactory.Close();
        }

        [TestMethod]
        public void test_account_manger_as_service()
        {
            ChannelFactory<IAccountService> channelFactory = new ChannelFactory<IAccountService>("");
            IAccountService proxy = channelFactory.CreateChannel();
            (proxy as ICommunicationObject).Open();
            channelFactory.Close();
        }
    }
}
