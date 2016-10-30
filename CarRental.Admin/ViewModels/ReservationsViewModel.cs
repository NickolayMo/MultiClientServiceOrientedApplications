using Core.Common.Contracts;
using Core.Common.UI.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Admin.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ReservationsViewModel:ViewModelBase
    {
        private IServiceFactory _ServiceFactory;
        [ImportingConstructor]
        public ReservationsViewModel(IServiceFactory serviceFactory)
        {
            _ServiceFactory = serviceFactory;             
        }

        public override string ViewTitle
        {
            get
            {
                return "Reservation";
            }
        }
    }
}
