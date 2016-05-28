using Core.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Web.Http.Dependencies;

namespace CarRental.Web.Core
{
    internal class MefApiDependencyResolver : IDependencyResolver
    {
        private CompositionContainer container;

        public MefApiDependencyResolver(CompositionContainer container)
        {
            this.container = container;
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public void Dispose()
        {
            
        }

        public object GetService(Type serviceType)
        {
            return container.GetExportedValueByType(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return container.GetExportedValuesByType(serviceType);
        }
    }
}