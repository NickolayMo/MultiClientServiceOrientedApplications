using Core.Common.UI.Core;
using System.ComponentModel.Composition;

namespace CarRental.Admin.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class MainViewModel : ViewModelBase
    {
        [Import]
        public DashboardViewModel DashboardViewModel { get; private set; }

        [Import]
        public RentalsViewModel RentalsViewModel { get; private set; }

        [Import]
        public ReservationsViewModel ReservationsViewModel { get; private set; }

        [Import]
        public MaintainCarsViewModel MaintainCarsViewModel { get; private set; }

    }
}
