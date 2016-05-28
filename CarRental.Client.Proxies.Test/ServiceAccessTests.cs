using CarRental.Client.Proxies.Proxies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CarRental.Client.Proxies.Test
{
    [TestClass]
    public class ServiceAccessTests
    {
        [TestMethod]
        public void test_inventory_client_connect()
        {
            InventoryClient proxy = new InventoryClient();
            proxy.Open();
        }
        [TestMethod]
        public void test_account_client_connect()
        {
            AccountClient proxy = new AccountClient();
            proxy.Open();
        }
        [TestMethod]
        public void test_rental_client_connect()
        {
            RentalClient proxy = new RentalClient();
            proxy.Open();
        }
    }
}
