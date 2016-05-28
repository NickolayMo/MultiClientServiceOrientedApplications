using CarRental.Client.Proxies.Proxies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Client.Bootstrapper
{
    public static class MEFLoader
    {
        public static CompositionContainer Init()
        {
            return Init(null);
        }

        public static CompositionContainer Init(ICollection<ComposablePartCatalog> parts)
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(InventoryClient).Assembly));
            if (parts != null)
            {
                foreach (var part in parts)
                {
                    catalog.Catalogs.Add(part);
                }
            }
            
            CompositionContainer container = new CompositionContainer(catalog);
            return container;
        }
    }
}
