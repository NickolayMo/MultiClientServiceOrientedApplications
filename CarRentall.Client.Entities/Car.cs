using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Core;
using FluentValidation;

namespace CarRental.Client.Entities
{

    public class Car : ObjectBase
    {
        private int _carId;
        private string _description;
        private int _year;
        private decimal _rentalPrice;
        private string _color;
        private bool _currentlyRented;

        public int CarId
        {
            get
            {
                return _carId;
            }
            set
            {
                if (_carId != value)
                {
                    _carId = value;
                    //OnPropertyChanged("CarId");
                    OnPropertyChanged(() => CarId);

                }

            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged(() => Description);
            }
        }


        public int Year
        {
            get
            {
                return _year;
            }
            set
            {
                _year = value;
                OnPropertyChanged(() => Year);
            }
        }


        public bool CurrentlyRented
        {
            get
            {
                return _currentlyRented;
            }
            set
            {
                _currentlyRented = value;
                OnPropertyChanged(() => CurrentlyRented);
            }
        }


        public decimal RentalPrice
        {
            get
            {
                return _rentalPrice;
            }
            set
            {
                _rentalPrice = value;
                OnPropertyChanged(() => RentalPrice);
            }
        }


        public string Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                OnPropertyChanged(() => Color);
            }
        }




        class CarValidator : AbstractValidator<Car>
        {
            public CarValidator()
            {
                RuleFor(obj => obj.Description).NotEmpty();
                RuleFor(obj => obj.Color).NotEmpty();
                RuleFor(obj => obj.RentalPrice).GreaterThan(0);
                RuleFor(obj => obj.Year).GreaterThan(2000).LessThanOrEqualTo(DateTime.Now.Year+1);
            }

        }
        protected override IValidator GetValidator()
        {
            return new CarValidator();
        }
    }
}
