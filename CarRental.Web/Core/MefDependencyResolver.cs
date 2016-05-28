using Core.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Web.Mvc;

namespace CarRental.Web.Core
{
    public class MefDependencyResolver : IDependencyResolver
    {
        private CompositionContainer container;

        public MefDependencyResolver(CompositionContainer container)
        {
            this.container = container;
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