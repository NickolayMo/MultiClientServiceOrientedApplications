using Core.Common.Contracts;
using System.Collections.Generic;

namespace CarRental.Web.Core
{
    public interface IServiceAwareController
    {
        void RegisterDisposableServices(List<IServiceContract> disposableServices);
        List<IServiceContract> DisposableServices { get; }
    }
}