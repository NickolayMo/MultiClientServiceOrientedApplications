using Core.Common.Core;
using System;

namespace CarRental.Client.Entities
{
    public class Rental:ObjectBase
    {
        private int _rentalId;
        private int _accountId;
        private int _carId;
        private DateTime _dateRented;
        private DateTime _dateDue;
        private DateTime? _dateReturned;

        public int RentalId
        {
            get
            {
                return _rentalId;
            }
            set
            {
                _rentalId = value;
                OnPropertyChanged(() => RentalId);
            }
        }

        public int AccountId
        {
            get
            {
                return _accountId;
            }
            set
            {
                _accountId = value;
                OnPropertyChanged(() => AccountId);
            }
        }
        public int CarId
        {
            get
            {
                return _carId;
            }
            set
            {
                _carId = value;
                OnPropertyChanged(() => CarId);
            }
        }

        public DateTime DateRented
        {
            get
            {
                return _dateRented;
            }
            set
            {
                _dateRented = value;
                OnPropertyChanged(() => DateRented);
            }
        }

        public DateTime DateDue
        {
            get
            {
                return _dateDue;
            }
            set
            {
                _dateDue = value;
                OnPropertyChanged(() => DateDue);
            }
        }

        public DateTime? DateReturned
        {
            get
            {
                return _dateReturned;
            }
            set
            {
                _dateReturned = value;
                OnPropertyChanged(() => DateReturned);
            }
        }
    }
}
