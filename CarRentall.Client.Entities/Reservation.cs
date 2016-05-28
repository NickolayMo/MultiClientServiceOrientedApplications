using Core.Common.Core;
using System;

namespace CarRental.Client.Entities
{
    public class Reservation:ObjectBase
    {

       
        private int _accountId;
        private int _carId;
        private DateTime _rentalDate;
        private DateTime _returnDate;        
        private int _reservationId;

        public int ReservationId
        {
            get
            {
                return _reservationId;
            }
            set
            {
                _reservationId = value;
                OnPropertyChanged(() => ReservationId);
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

        public DateTime RentalDate
        {
            get
            {
                return _rentalDate;
            }
            set
            {
                _rentalDate = value;
                OnPropertyChanged(() => RentalDate);
            }
        }

        public DateTime ReturnDate
        {
            get
            {
                return _returnDate;
            }
            set
            {
                _returnDate = value;
                OnPropertyChanged(() => ReturnDate);
            }
        }

       
    }
}
