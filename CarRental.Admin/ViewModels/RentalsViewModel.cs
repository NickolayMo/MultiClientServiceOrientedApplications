using Core.Common.Contracts;
using Core.Common.UI.Core;
using System.ComponentModel.Composition;

namespace CarRental.Admin.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class RentalsViewModel:ViewModelBase
    {
        private IServiceFactory _ServiceFactory;

        [ImportingConstructor]
        public RentalsViewModel(IServiceFactory serviceFactory)
        {
            _ServiceFactory = serviceFactory;
        }

        public override string ViewTitle
        {
            get
            {
                return "Rentals";
            }
        }
    }
}
