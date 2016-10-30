using Core.Common.Contracts;
using Core.Common.UI.Core;
using System.ComponentModel.Composition;

namespace CarRental.Admin.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class DashboardViewModel: ViewModelBase
    {
        [ImportingConstructor]
        public DashboardViewModel(IServiceFactory serviceFactory)
        {
            _ServiceFactory = serviceFactory;
        }
        IServiceFactory _ServiceFactory;
        public override string ViewTitle
        {
            get
            {
                return "Dashboard";
            }
        }
    }
}