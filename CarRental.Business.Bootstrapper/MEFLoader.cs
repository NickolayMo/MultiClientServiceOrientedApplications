using System.ComponentModel.Composition.Hosting;
using CarRental.Data.DataRepositories;
using CarRental.Business.BusinessEngines;

namespace CarRental.Business.Bootstrapper
{
    public static class MEFLoader
    {
        public static CompositionContainer Init()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(CarRepository).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(CarRentalEngine).Assembly));
            CompositionContainer container = new CompositionContainer(catalog);
            return container;
        }
    }
}
