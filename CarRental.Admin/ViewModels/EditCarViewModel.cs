using CarRental.Admin.Support;
using CarRental.Client.Contracts.ServiceContracts;
using CarRental.Client.Entities;
using Core.Common.Contracts;
using Core.Common.Core;
using Core.Common.UI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Admin.ViewModels
{
    public class EditCarViewModel : ViewModelBase
    {
        private Car _Car;
        private IServiceFactory _ServiceFactory;

        public EditCarViewModel(IServiceFactory serviceFactory, Car car)
        {
            _ServiceFactory = serviceFactory;
            _Car = new Car()
            {
                CarId = car.CarId,
                Description = car.Description,
                Color = car.Color,
                Year = car.Year,
                RentalPrice = car.RentalPrice
            };
            _Car.CleanAll();

            SaveCommand = new DelegateCommand<Object>(OnSaveCommandExecute, OnSaveCommandCanExecute);
            CancelCommand = new DelegateCommand<Object>(OnCancelCommandExecute);
        }

        private void OnCancelCommandExecute(object arg)
        {
            if (CancelEditCar != null)
            {
                CancelEditCar(this, EventArgs.Empty);
            }
        }

        private bool OnSaveCommandCanExecute(object obj)
        {
            return _Car.IsDirty;
        }

        private void OnSaveCommandExecute(object obj)
        {
            ValidateModel();
            if (IsValid)
            {
                WithClient<IInventoryService>(_ServiceFactory.CreateClient<IInventoryService>(), inventoryClient => {
                    bool isNew = (_Car.CarId == 0);
                    var savedCar = inventoryClient.UpdateCar(_Car);
                    if (savedCar != null)
                    {
                        if (CarUpdated != null)
                        {
                            CarUpdated(this, new CarEventArgs(savedCar, isNew));
                        }
                    }
                });
            }
        }

        public DelegateCommand<object> CancelCommand { get; private set; }
        public DelegateCommand<object> SaveCommand { get; private set; }

        public Car Car
        {
            get
            {
                return _Car;
            }

        }

        public event EventHandler CancelEditCar;
        public event EventHandler<CarEventArgs> CarUpdated;
        protected override void AddModel(List<ObjectBase> models)
        {
            models.Add(Car);
        }
    }
}
