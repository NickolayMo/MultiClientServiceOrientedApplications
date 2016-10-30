using CarRental.Client.Contracts.ServiceContracts;
using CarRental.Client.Entities;
using Core.Common.Contracts;
using Core.Common.UI.Core;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System;
using System.ComponentModel;
using System.ServiceModel;
using Core.Common.Misc;
using CarRental.Admin.Support;
using System.Linq;

namespace CarRental.Admin.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class MaintainCarsViewModel : ViewModelBase
    {
        private IServiceFactory _ServiceFactory;
        [ImportingConstructor]
        public MaintainCarsViewModel(IServiceFactory serviceFactory)
        {
            _ServiceFactory = serviceFactory;

            EditCarCommand = new DelegateCommand<Car>(OnEditCarCommand);
            AddCarCommand = new DelegateCommand<object>(OnAddCarCommand);
            DeleteCarCommand = new DelegateCommand<Car>(OnDeleteCarCommand);

        }
        
        private void OnDeleteCarCommand(Car obj)
        {
            bool carIsRented = false;
            WithClient<IRentalService>(_ServiceFactory.CreateClient<IRentalService>(), rentalClient => {
                carIsRented = rentalClient.IsCarCurrentlyRented(obj.CarId);

            });
            if (!carIsRented)
            {
                CancelEventArgs args = new CancelEventArgs();
                if (ConfirmDelete != null)
                {
                    ConfirmDelete(this, args);
                }
                if (!args.Cancel)
                {
                    try
                    {
                        WithClient<IInventoryService>(_ServiceFactory.CreateClient<IInventoryService>(), inventoryClient =>
                        {
                            inventoryClient.DeleteCar(obj.CarId);
                            _Cars.Remove(obj);
                        });
                    }
                    catch (FaultException e)
                    {
                        if (ErrorOccured != null)
                        {
                            ErrorOccured(this, new ErrorMessageEventArgs(e.Message));
                        }
                        
                    }
                    catch(Exception e)
                    {
                        if (ErrorOccured != null)
                        {
                            ErrorOccured(this, new ErrorMessageEventArgs(e.Message));
                        }
                    }
                }
                else
                {
                    if (ErrorOccured != null)
                    {
                        ErrorOccured(this, new ErrorMessageEventArgs("Cannot delete this car. It is currently rented."));
                    }

                }
            }
        }

        public DelegateCommand<Car> EditCarCommand { get; private set; }
        public DelegateCommand<object> AddCarCommand { get; private set; }
        public DelegateCommand<Car> DeleteCarCommand { get; private set; }

        public event CancelEventHandler ConfirmDelete;
        public event EventHandler<ErrorMessageEventArgs> ErrorOccured;
        public override string ViewTitle
        {
            get
            {
                return "Mantain cars";
            }
        }

        private ObservableCollection<Car> _Cars;
        private EditCarViewModel _CurrentCarViewModel;

        public ObservableCollection<Car> Cars
        {
            get
            {
                return _Cars;
            }
            set
            {
                _Cars = value;
                OnPropertyChanged(() => Cars, false);
            }
        }

        public EditCarViewModel CurrentCarViewModel
        {
           get
            {
                return _CurrentCarViewModel;
            }
            set
            {
                if (_CurrentCarViewModel != value)
                {
                    _CurrentCarViewModel = value;
                    OnPropertyChanged(() => CurrentCarViewModel, false);
                }
            }
        }

        protected override void OnViewLoaded()
        {
            _Cars = new ObservableCollection<Car>();
            WithClient<IInventoryService>(_ServiceFactory.CreateClient<IInventoryService>(), invetoryClient =>
            {
                Car[] cars = invetoryClient.GetAllCars();
                if (cars != null)
                {
                    foreach (var car in cars)
                    {
                        _Cars.Add(car);
                    }
                }
            });
        }
        private void OnEditCarCommand(Car car)
        {
            if (car != null)
            {
                CurrentCarViewModel = new EditCarViewModel(_ServiceFactory, car);
                CurrentCarViewModel.CarUpdated += CurrentCarViewModel_CarUpdated;
                CurrentCarViewModel.CancelEditCar += CurrentCarViewModel_CancelEditCar;
            }

        }
        private void OnAddCarCommand(object obj)
        {
            Car car = new Car();
            CurrentCarViewModel = new EditCarViewModel(_ServiceFactory, car);
            CurrentCarViewModel.CarUpdated += CurrentCarViewModel_CarUpdated;
            CurrentCarViewModel.CancelEditCar += CurrentCarViewModel_CancelEditCar;
        }

        private void CurrentCarViewModel_CancelEditCar(object sender, EventArgs e)
        {
            CurrentCarViewModel = null;
           
        }

        private void CurrentCarViewModel_CarUpdated(object sender, CarEventArgs e)
        {
            if (e.IsNew)
            {
                Car car = _Cars.Where(item => item.CarId == e.Car.CarId).FirstOrDefault();
                if (car != null)
                {
                    car.Description = e.Car.Description;
                    car.Color = e.Car.Color;
                    car.Year = e.Car.Year;
                    car.RentalPrice = e.Car.RentalPrice;
                }
            }
            else
            {
                _Cars.Add(e.Car);
            }
            CurrentCarViewModel = null;
        }
    }

    
}
